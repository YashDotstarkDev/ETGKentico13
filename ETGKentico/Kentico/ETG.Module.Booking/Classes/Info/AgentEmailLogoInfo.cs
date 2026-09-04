using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(AgentEmailLogoInfo), AgentEmailLogoInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="AgentEmailLogoInfo"/>.
    /// </summary>
    [Serializable]
    public partial class AgentEmailLogoInfo : AbstractInfo<AgentEmailLogoInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.agentemaillogo";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(AgentEmailLogoInfoProvider), OBJECT_TYPE, "ETG.AgentEmailLogo", "AgentEmailLogoID", "AgentEmailLogoLastModified", "AgentEmailLogoGuid", null, "AgentEmailEmailAddress", null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("AgentEmailAgentLogoID", "etg.agentlogo", ObjectDependencyEnum.Required),
            },
        };


        /// <summary>
        /// Agent email logo ID.
        /// </summary>
        [DatabaseField]
        public virtual int AgentEmailLogoID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("AgentEmailLogoID"), 0);
            }
            set
            {
                SetValue("AgentEmailLogoID", value);
            }
        }


        /// <summary>
        /// Agent email agent logo ID.
        /// </summary>
        [DatabaseField]
        public virtual int AgentEmailAgentLogoID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("AgentEmailAgentLogoID"), 0);
            }
            set
            {
                SetValue("AgentEmailAgentLogoID", value);
            }
        }


        /// <summary>
        /// Agent email email address.
        /// </summary>
        [DatabaseField]
        public virtual string AgentEmailEmailAddress
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AgentEmailEmailAddress"), String.Empty);
            }
            set
            {
                SetValue("AgentEmailEmailAddress", value);
            }
        }


        /// <summary>
        /// Agent email logo.
        /// </summary>
        [DatabaseField]
        public virtual string AgentEmailLogo
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AgentEmailLogo"), String.Empty);
            }
            set
            {
                SetValue("AgentEmailLogo", value, String.Empty);
            }
        }


        /// <summary>
        /// Agent email logo guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid AgentEmailLogoGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("AgentEmailLogoGuid"), Guid.Empty);
            }
            set
            {
                SetValue("AgentEmailLogoGuid", value);
            }
        }


        /// <summary>
        /// Agent email logo last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime AgentEmailLogoLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("AgentEmailLogoLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("AgentEmailLogoLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            AgentEmailLogoInfoProvider.DeleteAgentEmailLogoInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            AgentEmailLogoInfoProvider.SetAgentEmailLogoInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected AgentEmailLogoInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="AgentEmailLogoInfo"/> class.
        /// </summary>
        public AgentEmailLogoInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="AgentEmailLogoInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public AgentEmailLogoInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}