using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;

[assembly: RegisterObjectType(typeof(RoleDestinationLinkInfo), RoleDestinationLinkInfo.OBJECT_TYPE)]

namespace ETG.Module.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="RoleDestinationLinkInfo"/>.
    /// </summary>
    [Serializable]
    public partial class RoleDestinationLinkInfo : AbstractInfo<RoleDestinationLinkInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.roledestinationlink";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(RoleDestinationLinkInfoProvider), OBJECT_TYPE, "ETG.RoleDestinationLink", "RoleDestinationLinkID", "RoleDestinationLinkLastModified", "RoleDestinationRoleGuid", "RoleDestinationName", "RoleDestinationName", null, null, null, null)
        {
            ModuleName = "ETG",
            TouchCacheDependencies = true,
            ImportExportSettings =
            {
                IsExportable = true, // Makes the data of the custom Office class exportable
                AllowSingleExport = true, // Allows export of single office objects from the office listing page
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the global objects export interface
                    new ObjectTreeLocation(GLOBAL, "RoleDestinationName")
                }
            },
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization, // Enables logging of staging tasks for changes made to Office objects
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the 'Global objects' section of the staging object tree
                    new ObjectTreeLocation(GLOBAL, "RoleDestinationName")
                }
            }
        };


        /// <summary>
        /// Role destination link ID.
        /// </summary>
        [DatabaseField]
        public virtual int RoleDestinationLinkID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("RoleDestinationLinkID"), 0);
            }
            set
            {
                SetValue("RoleDestinationLinkID", value);
            }
        }


        /// <summary>
        /// Role destination name.
        /// </summary>
        [DatabaseField]
        public virtual string RoleDestinationName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("RoleDestinationName"), String.Empty);
            }
            set
            {
                SetValue("RoleDestinationName", value);
            }
        }


        /// <summary>
        /// Role destination role guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid RoleDestinationRoleGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("RoleDestinationRoleGuid"), Guid.Empty);
            }
            set
            {
                SetValue("RoleDestinationRoleGuid", value);
            }
        }


        /// <summary>
        /// Role destination destinations.
        /// </summary>
        [DatabaseField]
        public virtual string RoleDestinationDestinations
        {
            get
            {
                return ValidationHelper.GetString(GetValue("RoleDestinationDestinations"), String.Empty);
            }
            set
            {
                SetValue("RoleDestinationDestinations", value);
            }
        }


        /// <summary>
        /// Role destination link guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid RoleDestinationLinkGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("RoleDestinationLinkGuid"), Guid.Empty);
            }
            set
            {
                SetValue("RoleDestinationLinkGuid", value);
            }
        }


        /// <summary>
        /// Role destination link last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime RoleDestinationLinkLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("RoleDestinationLinkLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("RoleDestinationLinkLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            RoleDestinationLinkInfoProvider.DeleteRoleDestinationLinkInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            RoleDestinationLinkInfoProvider.SetRoleDestinationLinkInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected RoleDestinationLinkInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="RoleDestinationLinkInfo"/> class.
        /// </summary>
        public RoleDestinationLinkInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="RoleDestinationLinkInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public RoleDestinationLinkInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}