using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(AgentLogoInfo), AgentLogoInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="AgentLogoInfo"/>.
    /// </summary>
    [Serializable]
    public partial class AgentLogoInfo : AbstractInfo<AgentLogoInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.agentlogo";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(AgentLogoInfoProvider), OBJECT_TYPE, "ETG.AgentLogo", "AgentLogoID", "AgentLogoLastModified", "AgentLogoGuid", null, "AgentLogoAgencyName", null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Agent logo ID.
        /// </summary>
        [DatabaseField]
        public virtual int AgentLogoID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("AgentLogoID"), 0);
            }
            set
            {
                SetValue("AgentLogoID", value);
            }
        }


        /// <summary>
        /// Agent logo agency name.
        /// </summary>
        [DatabaseField]
        public virtual string AgentLogoAgencyName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AgentLogoAgencyName"), String.Empty);
            }
            set
            {
                SetValue("AgentLogoAgencyName", value);
            }
        }


        /// <summary>
        /// Agent logo domain.
        /// </summary>
        [DatabaseField]
        public virtual string AgentLogoDomain
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AgentLogoDomain"), String.Empty);
            }
            set
            {
                SetValue("AgentLogoDomain", value);
            }
        }


        /// <summary>
        /// Agent logo url.
        /// </summary>
        [DatabaseField]
        public virtual string AgentLogoUrl
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AgentLogoUrl"), String.Empty);
            }
            set
            {
                SetValue("AgentLogoUrl", value);
            }
        }


        /// <summary>
        /// Agent logo emails.
        /// </summary>
        [DatabaseField]
        public virtual string AgentLogoEmails
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AgentLogoEmails"), String.Empty);
            }
            set
            {
                SetValue("AgentLogoEmails", value, String.Empty);
            }
        }


        /// <summary>
        /// Agent logo guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid AgentLogoGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("AgentLogoGuid"), Guid.Empty);
            }
            set
            {
                SetValue("AgentLogoGuid", value);
            }
        }


        /// <summary>
        /// Agent logo last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime AgentLogoLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("AgentLogoLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("AgentLogoLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            AgentLogoInfoProvider.DeleteAgentLogoInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            AgentLogoInfoProvider.SetAgentLogoInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected AgentLogoInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="AgentLogoInfo"/> class.
        /// </summary>
        public AgentLogoInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="AgentLogoInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public AgentLogoInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}