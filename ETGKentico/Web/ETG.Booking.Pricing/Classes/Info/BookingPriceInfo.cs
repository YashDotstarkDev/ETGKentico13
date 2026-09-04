using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Classes.Providers;

[assembly: RegisterObjectType(typeof(BookingPriceInfo), BookingPriceInfo.OBJECT_TYPE)]

namespace ETG.Booking.Pricing.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="BookingPriceInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BookingPriceInfo : AbstractInfo<BookingPriceInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.bookingprice";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BookingPriceInfoProvider), OBJECT_TYPE, "ETG.BookingPrice", "BookingPriceID", "BookingPriceLastModified", "BookingPriceGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Booking price ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingPriceID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingPriceID"), 0);
            }
            set
            {
                SetValue("BookingPriceID", value);
            }
        }


        /// <summary>
        /// Booking price tour code.
        /// </summary>
        [DatabaseField]
        public virtual string BookingPriceTourCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingPriceTourCode"), String.Empty);
            }
            set
            {
                SetValue("BookingPriceTourCode", value);
            }
        }


        /// <summary>
        /// Booking price start date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingPriceStartDate
        {
            get
            {
                return ValidationHelper.GetDate(GetValue("BookingPriceStartDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingPriceStartDate", value);
            }
        }


        /// <summary>
        /// Booking price end date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingPriceEndDate
        {
            get
            {
                return ValidationHelper.GetDate(GetValue("BookingPriceEndDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingPriceEndDate", value);
            }
        }


        /// <summary>
        /// Booking price twin share price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPriceTwinSharePrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPriceTwinSharePrice"), 0d);
            }
            set
            {
                SetValue("BookingPriceTwinSharePrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking price single supplemental cost.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPriceSingleSupplementalCost
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPriceSingleSupplementalCost"), 0d);
            }
            set
            {
                SetValue("BookingPriceSingleSupplementalCost", value, 0d);
            }
        }


        /// <summary>
        /// Booking price pre night twin price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPricePreNightTwinPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPricePreNightTwinPrice"), 0d);
            }
            set
            {
                SetValue("BookingPricePreNightTwinPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking price post night twin price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPricePostNightTwinPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPricePostNightTwinPrice"), 0d);
            }
            set
            {
                SetValue("BookingPricePostNightTwinPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking price pre night single price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPricePreNightSinglePrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPricePreNightSinglePrice"), 0d);
            }
            set
            {
                SetValue("BookingPricePreNightSinglePrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking price post night single price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPricePostNightSinglePrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPricePostNightSinglePrice"), 0d);
            }
            set
            {
                SetValue("BookingPricePostNightSinglePrice", value, 0d);
            }
        }


        /// <summary>
        /// Balance due days.
        /// </summary>
        [DatabaseField]
        public virtual int BalanceDueDays
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BalanceDueDays"), 0);
            }
            set
            {
                SetValue("BalanceDueDays", value, 0);
            }
        }


        /// <summary>
        /// Second instalment days.
        /// </summary>
        [DatabaseField]
        public virtual int SecondInstalmentDays
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SecondInstalmentDays"), 0);
            }
            set
            {
                SetValue("SecondInstalmentDays", value, 0);
            }
        }


        /// <summary>
        /// Second instalment percent.
        /// </summary>
        [DatabaseField]
        public virtual double SecondInstalmentPercent
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("SecondInstalmentPercent"), 0d);
            }
            set
            {
                SetValue("SecondInstalmentPercent", value, 0d);
            }
        }


        /// <summary>
        /// Is enabled.
        /// </summary>
        [DatabaseField]
        public virtual bool IsEnabled
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("IsEnabled"), true);
            }
            set
            {
                SetValue("IsEnabled", value);
            }
        }


        /// <summary>
        /// Booking price guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BookingPriceGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BookingPriceGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BookingPriceGuid", value);
            }
        }


        /// <summary>
        /// Booking price last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingPriceLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BookingPriceLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingPriceLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BookingPriceInfoProvider.DeleteBookingPriceInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BookingPriceInfoProvider.SetBookingPriceInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BookingPriceInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BookingPriceInfo"/> class.
        /// </summary>
        public BookingPriceInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BookingPriceInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BookingPriceInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}