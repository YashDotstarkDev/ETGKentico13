using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(BookingQuoteInfo), BookingQuoteInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="BookingQuoteInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BookingQuoteInfo : AbstractInfo<BookingQuoteInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.bookingquote";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BookingQuoteInfoProvider), OBJECT_TYPE, "ETG.BookingQuote", "BookingQuoteID", "BookingQuoteLastModified", "BookingQuoteGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Booking quote ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingQuoteID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingQuoteID"), 0);
            }
            set
            {
                SetValue("BookingQuoteID", value);
            }
        }


        /// <summary>
        /// Booking quote tour code.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteTourCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteTourCode"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteTourCode", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote custom data.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteCustomData
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteCustomData"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteCustomData", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote customer ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingQuoteCustomerID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingQuoteCustomerID"), 0);
            }
            set
            {
                SetValue("BookingQuoteCustomerID", value, 0);
            }
        }


        /// <summary>
        /// Booking quote customer comments.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteCustomerComments
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteCustomerComments"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteCustomerComments", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote valid days.
        /// </summary>
        [DatabaseField]
        public virtual int BookingQuoteValidDays
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingQuoteValidDays"), 0);
            }
            set
            {
                SetValue("BookingQuoteValidDays", value, 0);
            }
        }


        /// <summary>
        /// Booking quote total price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingQuoteTotalPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingQuoteTotalPrice"), 0d);
            }
            set
            {
                SetValue("BookingQuoteTotalPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking quote net price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingQuoteNetPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingQuoteNetPrice"), 0d);
            }
            set
            {
                SetValue("BookingQuoteNetPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking quote agent price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingQuoteAgentPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingQuoteAgentPrice"), 0d);
            }
            set
            {
                SetValue("BookingQuoteAgentPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking quote topdog number.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteTopdogNumber
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteTopdogNumber"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteTopdogNumber", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote last email.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteLastEmail
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteLastEmail"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteLastEmail", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote utm campaign.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteUtmCampaign
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteUtmCampaign"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteUtmCampaign", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote utm content.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteUtmContent
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteUtmContent"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteUtmContent", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote utm medium.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteUtmMedium
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteUtmMedium"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteUtmMedium", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote utm source.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteUtmSource
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteUtmSource"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteUtmSource", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote utm term.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteUtmTerm
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteUtmTerm"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteUtmTerm", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote currency.
        /// </summary>
        [DatabaseField]
        public virtual string BookingQuoteCurrency
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingQuoteCurrency"), String.Empty);
            }
            set
            {
                SetValue("BookingQuoteCurrency", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking quote conversion rate.
        /// </summary>
        [DatabaseField]
        public virtual decimal BookingQuoteConversionRate
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("BookingQuoteConversionRate"), 0m);
            }
            set
            {
                SetValue("BookingQuoteConversionRate", value, 0m);
            }
        }


        /// <summary>
        /// Booking quote guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BookingQuoteGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BookingQuoteGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BookingQuoteGuid", value);
            }
        }


        /// <summary>
        /// Booking quote created.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingQuoteCreated
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BookingQuoteCreated"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingQuoteCreated", value, DateTimeHelper.ZERO_TIME);
            }
        }


        /// <summary>
        /// Booking quote last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingQuoteLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BookingQuoteLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingQuoteLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BookingQuoteInfoProvider.DeleteBookingQuoteInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BookingQuoteInfoProvider.SetBookingQuoteInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BookingQuoteInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BookingQuoteInfo"/> class.
        /// </summary>
        public BookingQuoteInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BookingQuoteInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BookingQuoteInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}