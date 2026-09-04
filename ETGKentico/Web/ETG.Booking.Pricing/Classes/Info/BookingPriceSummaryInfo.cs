using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Classes.Providers;

[assembly: RegisterObjectType(typeof(BookingPriceSummaryInfo), BookingPriceSummaryInfo.OBJECT_TYPE)]

namespace ETG.Booking.Pricing.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="BookingPriceSummaryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BookingPriceSummaryInfo : AbstractInfo<BookingPriceSummaryInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.bookingpricesummary";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BookingPriceSummaryInfoProvider), OBJECT_TYPE, "ETG.BookingPriceSummary", "BookingPriceSummaryID", "BookingPriceSummaryLastModified", "BookingPriceSummaryGuid", null, "BookingPriceSummaryTourCode", null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Booking price summary ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingPriceSummaryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingPriceSummaryID"), 0);
            }
            set
            {
                SetValue("BookingPriceSummaryID", value);
            }
        }


        /// <summary>
        /// Booking price summary tour code.
        /// </summary>
        [DatabaseField]
        public virtual string BookingPriceSummaryTourCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingPriceSummaryTourCode"), String.Empty);
            }
            set
            {
                SetValue("BookingPriceSummaryTourCode", value);
            }
        }


        /// <summary>
        /// Booking price summary tour lowest price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingPriceSummaryTourLowestPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingPriceSummaryTourLowestPrice"), 0d);
            }
            set
            {
                SetValue("BookingPriceSummaryTourLowestPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking price summary guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BookingPriceSummaryGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BookingPriceSummaryGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BookingPriceSummaryGuid", value);
            }
        }


        /// <summary>
        /// Booking price summary last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingPriceSummaryLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BookingPriceSummaryLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingPriceSummaryLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BookingPriceSummaryInfoProvider.DeleteBookingPriceSummaryInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BookingPriceSummaryInfoProvider.SetBookingPriceSummaryInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BookingPriceSummaryInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BookingPriceSummaryInfo"/> class.
        /// </summary>
        public BookingPriceSummaryInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BookingPriceSummaryInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BookingPriceSummaryInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}