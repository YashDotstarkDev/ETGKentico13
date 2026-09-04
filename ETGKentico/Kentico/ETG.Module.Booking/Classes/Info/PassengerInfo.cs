using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(PassengerInfo), PassengerInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="PassengerInfo"/>.
    /// </summary>
    [Serializable]
    public partial class PassengerInfo : AbstractInfo<PassengerInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.passenger";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(PassengerInfoProvider), OBJECT_TYPE, "ETG.Passenger", "PassengerID", "PassengerLastModified", "PassengerGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Passenger ID.
        /// </summary>
        [DatabaseField]
        public virtual int PassengerID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PassengerID"), 0);
            }
            set
            {
                SetValue("PassengerID", value);
            }
        }


        /// <summary>
        /// Passenger customer ID.
        /// </summary>
        [DatabaseField]
        public virtual int PassengerCustomerID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PassengerCustomerID"), 0);
            }
            set
            {
                SetValue("PassengerCustomerID", value);
            }
        }


        /// <summary>
        /// Passenger first name.
        /// </summary>
        [DatabaseField]
        public virtual string PassengerFirstName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PassengerFirstName"), String.Empty);
            }
            set
            {
                SetValue("PassengerFirstName", value);
            }
        }


        /// <summary>
        /// Passenger last name.
        /// </summary>
        [DatabaseField]
        public virtual string PassengerLastName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PassengerLastName"), String.Empty);
            }
            set
            {
                SetValue("PassengerLastName", value);
            }
        }


        /// <summary>
        /// Passenger middle name.
        /// </summary>
        [DatabaseField]
        public virtual string PassengerMiddleName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PassengerMiddleName"), String.Empty);
            }
            set
            {
                SetValue("PassengerMiddleName", value, String.Empty);
            }
        }


        /// <summary>
        /// Passenger title.
        /// </summary>
        [DatabaseField]
        public virtual string PassengerTitle
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PassengerTitle"), String.Empty);
            }
            set
            {
                SetValue("PassengerTitle", value, String.Empty);
            }
        }


        /// <summary>
        /// Passenger date of birth.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PassengerDateOfBirth
        {
            get
            {
                return ValidationHelper.GetDate(GetValue("PassengerDateOfBirth"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PassengerDateOfBirth", value, DateTimeHelper.ZERO_TIME);
            }
        }


        /// <summary>
        /// Passenger guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid PassengerGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PassengerGuid"), Guid.Empty);
            }
            set
            {
                SetValue("PassengerGuid", value);
            }
        }


        /// <summary>
        /// Passenger last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PassengerLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PassengerLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PassengerLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            PassengerInfoProvider.DeletePassengerInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            PassengerInfoProvider.SetPassengerInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected PassengerInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="PassengerInfo"/> class.
        /// </summary>
        public PassengerInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="PassengerInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public PassengerInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}