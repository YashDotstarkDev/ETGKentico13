using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Classes.Providers;

[assembly: RegisterObjectType(typeof(BookingRoomOptionInfo), BookingRoomOptionInfo.OBJECT_TYPE)]

namespace ETG.Booking.Pricing.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="BookingRoomOptionInfo"/>.
    /// </summary>
    [Serializable]
    public partial class BookingRoomOptionInfo : AbstractInfo<BookingRoomOptionInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.bookingroomoption";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(BookingRoomOptionInfoProvider), OBJECT_TYPE, "ETG.BookingRoomOption", "BookingRoomOptionID", "BookingRoomOptionLastModified", "BookingRoomOptionGuid", null, "BookingRoomOptionLabel", null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("BookingRoomOptionBookingPriceID", "etg.bookingprice", ObjectDependencyEnum.Required),
            },
        };


        /// <summary>
        /// Booking room option ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingRoomOptionID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingRoomOptionID"), 0);
            }
            set
            {
                SetValue("BookingRoomOptionID", value);
            }
        }


        /// <summary>
        /// Booking room option label.
        /// </summary>
        [DatabaseField]
        public virtual string BookingRoomOptionLabel
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingRoomOptionLabel"), String.Empty);
            }
            set
            {
                SetValue("BookingRoomOptionLabel", value);
            }
        }


        /// <summary>
        /// Booking room option price per person.
        /// </summary>
        [DatabaseField]
        public virtual double BookingRoomOptionPricePerPerson
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingRoomOptionPricePerPerson"), 0d);
            }
            set
            {
                SetValue("BookingRoomOptionPricePerPerson", value);
            }
        }


        /// <summary>
        /// Booking room option label 2.
        /// </summary>
        [DatabaseField]
        public virtual string BookingRoomOptionLabel2
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BookingRoomOptionLabel2"), String.Empty);
            }
            set
            {
                SetValue("BookingRoomOptionLabel2", value, String.Empty);
            }
        }


        /// <summary>
        /// Booking room option price per person 2.
        /// </summary>
        [DatabaseField]
        public virtual double BookingRoomOptionPricePerPerson2
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingRoomOptionPricePerPerson2"), 0d);
            }
            set
            {
                SetValue("BookingRoomOptionPricePerPerson2", value, 0d);
            }
        }


        /// <summary>
        /// Booking room option type.
        /// </summary>
        [DatabaseField]
        public virtual int BookingRoomOptionType
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingRoomOptionType"), 0);
            }
            set
            {
                SetValue("BookingRoomOptionType", value);
            }
        }


        /// <summary>
        /// Booking room option order.
        /// </summary>
        [DatabaseField]
        public virtual int BookingRoomOptionOrder
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingRoomOptionOrder"), 0);
            }
            set
            {
                SetValue("BookingRoomOptionOrder", value);
            }
        }


        /// <summary>
        /// Booking room option twin pre night price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingRoomOptionTwinPreNightPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingRoomOptionTwinPreNightPrice"), 0d);
            }
            set
            {
                SetValue("BookingRoomOptionTwinPreNightPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking room option twin post night price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingRoomOptionTwinPostNightPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingRoomOptionTwinPostNightPrice"), 0d);
            }
            set
            {
                SetValue("BookingRoomOptionTwinPostNightPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking room option single pre night price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingRoomOptionSinglePreNightPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingRoomOptionSinglePreNightPrice"), 0d);
            }
            set
            {
                SetValue("BookingRoomOptionSinglePreNightPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking room option single post night price.
        /// </summary>
        [DatabaseField]
        public virtual double BookingRoomOptionSinglePostNightPrice
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("BookingRoomOptionSinglePostNightPrice"), 0d);
            }
            set
            {
                SetValue("BookingRoomOptionSinglePostNightPrice", value, 0d);
            }
        }


        /// <summary>
        /// Booking room option booking price ID.
        /// </summary>
        [DatabaseField]
        public virtual int BookingRoomOptionBookingPriceID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("BookingRoomOptionBookingPriceID"), 0);
            }
            set
            {
                SetValue("BookingRoomOptionBookingPriceID", value, 0);
            }
        }


        /// <summary>
        /// Booking room option guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid BookingRoomOptionGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("BookingRoomOptionGuid"), Guid.Empty);
            }
            set
            {
                SetValue("BookingRoomOptionGuid", value);
            }
        }


        /// <summary>
        /// Booking room option last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime BookingRoomOptionLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("BookingRoomOptionLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("BookingRoomOptionLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            BookingRoomOptionInfoProvider.DeleteBookingRoomOptionInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            BookingRoomOptionInfoProvider.SetBookingRoomOptionInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BookingRoomOptionInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="BookingRoomOptionInfo"/> class.
        /// </summary>
        public BookingRoomOptionInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="BookingRoomOptionInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public BookingRoomOptionInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}