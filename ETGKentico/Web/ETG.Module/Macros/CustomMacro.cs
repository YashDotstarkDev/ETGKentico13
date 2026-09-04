using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using CMS;
using CMS.Activities;
using CMS.Activities.Internal;
using CMS.Base;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.OnlineForms;
using CMS.SiteProvider;
using DocumentFormat.OpenXml.Spreadsheet;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Module.Macros;
using Newtonsoft.Json;

// Makes all methods in the 'CustomMacroMethods' container class available for string objects
[assembly: RegisterExtension(typeof(CustomMacro), typeof(string))]
// Registers methods from the 'CustomMacroMethods' container into the "String" macro namespace
[assembly: RegisterExtension(typeof(CustomMacro), typeof(StringNamespace))]
namespace ETG.Module.Macros
{
    public class CustomMacro : MacroMethodContainer
    {
        [MacroMethod(typeof(string), "return destination alias", 0)]
        [MacroMethodParam(0, "param1", typeof(string), "Empty")]

        public static object GetDestinationAlias(EvaluationContext context, params object[] parameters)
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }
            var nodeId = ValidationHelper.GetInteger(HttpContext.Current.Request["nodeid"], 0);
            
            if (nodeId == 0)
            {
                return string.Empty;
            }

            var tour = TourProvider.GetTours().OnCurrentSite().WhereEquals("NodeID", nodeId).Published(false).PublishedVersion(false).FirstOrDefault();

            if (tour == null)
            {
                return string.Empty;
            }

            var destination = DestinationProvider.GetDestinations().OnCurrentSite().WhereEquals("NodeGuid", tour.TourPrimaryCountry).Published(false).PublishedVersion(false).FirstOrDefault();

            if (destination != null)
            {
                return destination.NodeAlias;
            }

            var experience = ExperienceProvider.GetExperiences().OnCurrentSite().WhereEquals("NodeGuid", tour.TourPrimaryExperience).Published(false).PublishedVersion(false).FirstOrDefault();

            if (experience != null)
            {
                return experience.NodeAlias;
            }

            if (tour.TourIsCruise && tour.TourCruiseType > 0)
            {
                // Referenced from ETG.Data.Tour.Data.TourData.CruiseTypes
                List<Tuple<int, string, string>> CruiseTypes = new List<Tuple<int, string, string>>
                {
                    new Tuple<int, string, string>( 1, "Self-Drive", "icon-self-drive-cruising"),
                    new Tuple<int, string, string>( 2, "Barge", "icon-barge-cruising"),
                    new Tuple<int, string, string>( 3, "River", "icon-river-cruising"),
                    new Tuple<int, string, string>( 4, "Ocean", "icon-ocean-cruising"),
                };

                return CruiseTypes.FirstOrDefault(a => a.Item1.Equals(tour.TourCruiseType))?.Item2.Replace(" ", "-");
            }
            

            return string.Empty;

        }


        [MacroMethod(typeof(bool), "Returns true if the contact filled a form with the given field at least once so that the value contains expected value.", 3)]
        [MacroMethodParam(0, "formNameField", typeof(string), "Form name combined with form field name as '&lt;site name&gt;;&lt;form name&gt;;&lt;form field name&gt;'.")]
        [MacroMethodParam(1, "expectedValue", typeof(string), "Expected value.")]
        [MacroMethodParam(2, "lastXMinutes", typeof(int), "Constraint for last X minutes (if zero or negative value is given, no constraint is applied).")]
        public static object FilledFormFieldWithValueMinutes(EvaluationContext context, params object[] parameters)
        {
            ActivityInfo lastActivity = ActivityInfoProvider.GetActivities().WhereEquals(nameof(ActivityInfo.ActivityType), "bizformsubmit")
                .OrderByDescending(nameof(ActivityInfo.ActivityID)).FirstOrDefault();

            if (parameters.Length < 2 || parameters.Length > 3)
            {
                throw new NotSupportedException();
            }
            
            string[] array = ValidationHelper.GetString(parameters[0], string.Empty).Split(';');
            if (array.Length != 3)
            {
                throw new ArgumentException("Second parameter is expected in form <site name>;<form name>;<form field name>");
            }
            string siteName = array[0];
            string formName = array[1];
            string fieldName = array[2];
            string expectedValue = ValidationHelper.GetString(parameters[1], string.Empty);
            try
            {
                return FilledFormFieldWithValueMinutes(ValidationHelper.GetInteger(lastActivity.ActivityContactID, 0), siteName, formName, fieldName, expectedValue, (parameters.Length == 3) ? ValidationHelper.GetInteger(parameters[2], 0) : 0);
            }
            catch (ArgumentException ex)
            {
                Service.Resolve<IEventLogService>().LogException("FilledFormFieldWithValueMinutes", "RESOLVEMACRO", ex, 0, null, new LoggingPolicy(TimeSpan.FromMinutes(10.0)));
            }
            return false;
        }

        [MacroMethod(typeof(bool), "Check the last record of the form and return true if the condition is fulfilled.", 3)]
        [MacroMethodParam(0, "formNameField", typeof(string), "Form name combined with form field name as '&lt;site name&gt;;&lt;form name&gt;;&lt;form field name&gt;'.")]
        [MacroMethodParam(1, "expectedValue", typeof(string), "Expected value.")]
        public static object FilledFormFieldWithValue(EvaluationContext context, params object[] parameters)
        {
            string[] array = ValidationHelper.GetString(parameters[0], string.Empty).Split(';');
            if (array.Length != 3)
            {
                throw new ArgumentException("Second parameter is expected in form <site name>;<form name>;<form field name>");
            }
            string siteName = array[0];
            string formName = array[1];
            string fieldName = array[2];
            string expectedValue = ValidationHelper.GetString(parameters[1], string.Empty);
            try
            {
                return FilledFormFieldWithValue(siteName, formName, fieldName, expectedValue);
            }
            catch (ArgumentException ex)
            {
                Service.Resolve<IEventLogService>().LogException("FilledFormFieldWithValue", "RESOLVEMACRO", ex, 0, null, new LoggingPolicy(TimeSpan.FromMinutes(10.0)));
            }
            return false;
        }


        #region "Internal"

        internal static bool FilledFormFieldWithValue(string siteName, string formName, string fieldName, string expectedValue)
        {
            if (!ValidationHelper.IsCodeName(formName))
            {
                throw new ArgumentException("Is not valid code name", "formName");
            }
            if (!ValidationHelper.IsCodeName(fieldName))
            {
                throw new ArgumentException("Is not valid code name", "fieldName");
            }
            BizFormInfo bizFormInfo = AbstractInfo<BizFormInfo, IBizFormInfoProvider>.Provider.Get(ValidationHelper.GetCodeName(formName), SiteInfoProvider.GetSiteID(siteName));
            if (bizFormInfo == null)
            {
                throw new ArgumentException($"Form with given name '{formName}' cannot be found.", "formName");
            }
            DataClassInfo dataClassInfo = DataClassInfoProviderBase<DataClassInfoProvider>.GetDataClassInfo(bizFormInfo.FormClassID);
            FormInfo formInfo = new FormInfo(dataClassInfo.ClassFormDefinition);
            if (formInfo.GetFields<FormFieldInfo>().All((FormFieldInfo x) => x.Name != fieldName))
            {
                throw new ArgumentException($"Form '{formName}' does not contains given field '{fieldName}'.", "fieldName");
            }
            if (!formInfo.GetFields<FormFieldInfo>().Any((FormFieldInfo f) => f.DataType == "text" && f.Name == fieldName))
            {
                throw new ArgumentException("Only text fields are allowed.", "fieldName");
            }

            var lastRecord = BizFormItemProvider.GetItems(dataClassInfo.ClassName)
                .OrderByDescending("FormInserted")
                .FirstOrDefault();

            if (lastRecord != null)
            {
                string fieldValue = ValidationHelper.GetString(lastRecord.GetValue(fieldName), string.Empty);
                return fieldValue.Contains(expectedValue);
            }
            return false;
        }
        internal static bool FilledFormFieldWithValueMinutes(int contactId, string siteName, string formName, string fieldName, string expectedValue, int lastXMinutes)
        {
            if (contactId == 0)
            {
                throw new ArgumentNullException("contact");
            }
            if (!ValidationHelper.IsCodeName(formName))
            {
                throw new ArgumentException("Is not valid code name", "formName");
            }
            if (!ValidationHelper.IsCodeName(fieldName))
            {
                throw new ArgumentException("Is not valid code name", "fieldName");
            }
            BizFormInfo bizFormInfo = AbstractInfo<BizFormInfo, IBizFormInfoProvider>.Provider.Get(ValidationHelper.GetCodeName(formName), SiteInfoProvider.GetSiteID(siteName));
            if (bizFormInfo == null)
            {
                throw new ArgumentException($"Form with given name '{formName}' cannot be found.", "formName");
            }
            DataClassInfo dataClassInfo = DataClassInfoProviderBase<DataClassInfoProvider>.GetDataClassInfo(bizFormInfo.FormClassID);
            FormInfo formInfo = new FormInfo(dataClassInfo.ClassFormDefinition);
            if (formInfo.GetFields<FormFieldInfo>().All((FormFieldInfo x) => x.Name != fieldName))
            {
                throw new ArgumentException($"Form '{formName}' does not contains given field '{fieldName}'.", "fieldName");
            }
            if (!formInfo.GetFields<FormFieldInfo>().Any((FormFieldInfo f) => f.DataType == "text" && f.Name == fieldName))
            {
                throw new ArgumentException("Only text fields are allowed.", "fieldName");
            }
            ObjectQuery<ActivityInfo> objectQuery = AbstractInfo<ActivityInfo, IActivityInfoProvider>.Provider.Get().WhereEquals("ActivityContactID", contactId).WhereEquals("ActivityType", "bizformsubmit")
                .WhereEquals("ActivityItemID", bizFormInfo.FormID)
                .OnSite(bizFormInfo.FormSiteID)
            .Column("ActivityItemDetailID");

            if (lastXMinutes > 0)
            {
                objectQuery.NewerThan(TimeSpan.FromMinutes(lastXMinutes));
            }

            string name = formInfo.GetFields<FormFieldInfo>().First((FormFieldInfo x) => x.PrimaryKey).Name;

            return BizFormItemProvider.GetItems(dataClassInfo.ClassName).WhereIn(name, objectQuery).WhereContains(fieldName, expectedValue)
                .Any();
        }
        #endregion
    }
}