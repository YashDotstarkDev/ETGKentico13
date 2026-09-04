using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(BookingServiceInfo), BookingServiceInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="BookingServiceInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BookingServiceInfo : AbstractInfo<BookingServiceInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.bookingservice";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BookingServiceInfoProvider), OBJECT_TYPE, "ETG.BookingService", "BookingServiceID", "BookingServiceLastModified", "BookingServiceGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Booking service ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingServiceID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingServiceID"), 0);
            }
            set
            {
                SetValue("BookingServiceID", value);
            }
        }


        /// <summary>
        /// Booking service quote ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingServiceQuoteID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingServiceQuoteID"), 0);
            }
            set
            {
                SetValue("BookingServiceQuoteID", value);
            }
        }


        /// <summary>
        /// Booking service name.
        /// </summary>
        [DatabaseField]
        public virtual string BookingServiceName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingServiceName"), String.Empty);
            }
            set
            {
                SetValue("BookingServiceName", value);
            }
        }


        /// <summary>
        /// Booking service description.
        /// </summary>
        [DatabaseField]
        public virtual string BookingServiceDescription
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingServiceDescription"), String.Empty);
            }
            set
            {
                SetValue("BookingServiceDescription", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking service unit price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingServiceUnitPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingServiceUnitPrice"), 0d);
            }
            set
            {
                SetValue("BookingServiceUnitPrice", value);
            }
        }


        /// <summary>
        /// Booking service quantity.
        /// </summary>
        [DatabaseField]
        public virtual int BookingServiceQuantity
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingServiceQuantity"), 0);
            }
            set
            {
                SetValue("BookingServiceQuantity", value);
            }
        }


        /// <summary>
        /// Booking service start date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingServiceStartDate
        {
            get
            {
                return ValidationHelper.GetDate(GetValue("BookingServiceStartDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingServiceStartDate", value, DateTimeHelper.ZERO_TIME);
            }
        }


        /// <summary>
        /// Booking service guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BookingServiceGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BookingServiceGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BookingServiceGuid", value);
            }
        }


        /// <summary>
        /// Booking service last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingServiceLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BookingServiceLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingServiceLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BookingServiceInfoProvider.DeleteBookingServiceInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BookingServiceInfoProvider.SetBookingServiceInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BookingServiceInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BookingServiceInfo"/> class.
        /// </summary>
        public BookingServiceInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BookingServiceInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BookingServiceInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}