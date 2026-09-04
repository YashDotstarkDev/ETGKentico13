using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;

namespace CMSApp.CMSFormControls.Custom
{
    public partial class TourCodeControl : FormEngineUserControl
    {

        #region "Properties"

        /// <summary>
        /// Gets or sets the enabled state of the control.
        /// </summary>
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                txtTourCode.Enabled = value;
            }
        }


        /// <summary>
        /// Gets or sets field value.
        /// </summary>
        public override object Value
        {
            get { return txtTourCode.Text; }
            set { txtTourCode.Text = (string) value; }
        }


        /// <summary>
        /// Gets ClientID of the textbox with emailinput.
        /// </summary>
        public override string ValueElementID
        {
            get { return txtTourCode.ClientID; }
        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Page load.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Returns <c>true</c> if email input is empty or has correct format.
        /// </summary>
        /// <remarks>Also returns <c>true</c> if email input contains macro and is not used on live site.</remarks>>
        public override bool IsValid()
        {
            string tourCode = ValidationHelper.GetString(Value, String.Empty);
            var nodeID = ValidationHelper.GetInteger(Request["nodeid"], 0);

            if (string.IsNullOrWhiteSpace(tourCode))
            {
                ValidationError = "Tour code cannot be blank";
                return false;
            }

            if (nodeID == 0)
            {
                if (DocumentHelper.GetDocuments("ETG.Tour").OnCurrentSite().WhereLike("TourCode", $"{tourCode.Trim()}").Any())
                {
                    ValidationError = "Tour code needs to be unique";
                    return false;
                }
            }
            else
            {
                if (DocumentHelper.GetDocuments("ETG.Tour").OnCurrentSite().WhereLike("TourCode", $"{tourCode.Trim()}")
                    .WhereNotEquals("NodeID", nodeID).Any())
                {
                    ValidationError = "Tour code needs to be unique";
                    return false;
                }
            }

            return true;
        }



        #endregion
    }
}