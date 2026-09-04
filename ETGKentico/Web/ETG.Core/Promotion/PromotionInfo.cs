using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Core.Promotion;

[assembly: RegisterObjectType(typeof(PromotionInfo), PromotionInfo.OBJECT_TYPE)]

namespace ETG.Core.Promotion
{
     /// <summary>
    /// Data container class for <see cref="PromotionInfo"/>.
    /// </summary>
    [Serializable]
    public partial class PromotionInfo : AbstractInfo<PromotionInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.promotion";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(PromotionInfoProvider), OBJECT_TYPE, "ETG.Promotion", "PromotionID", "PromotionLastModified", "PromotionGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Promotion ID.
        /// </summary>
        [DatabaseField]
        public virtual int PromotionID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PromotionID"), 0);
            }
            set
            {
                SetValue("PromotionID", value);
            }
        }


        /// <summary>
        /// Promotion name.
        /// </summary>
        [DatabaseField]
        public virtual string PromotionName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PromotionName"), String.Empty);
            }
            set
            {
                SetValue("PromotionName", value);
            }
        }


        /// <summary>
        /// Promotion promo code.
        /// </summary>
        [DatabaseField]
        public virtual string PromotionPromoCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PromotionPromoCode"), String.Empty);
            }
            set
            {
                SetValue("PromotionPromoCode", value, String.Empty);
            }
        }


        /// <summary>
        /// Promotion destinations.
        /// </summary>
        [DatabaseField]
        public virtual string PromotionDestinations
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PromotionDestinations"), String.Empty);
            }
            set
            {
                SetValue("PromotionDestinations", value, String.Empty);
            }
        }


        /// <summary>
        /// Promotion email domains.
        /// </summary>
        [DatabaseField]
        public virtual string PromotionEmailDomains
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PromotionEmailDomains"), String.Empty);
            }
            set
            {
                SetValue("PromotionEmailDomains", value, String.Empty);
            }
        }


        /// <summary>
        /// Promotion from date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PromotionFromDate
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PromotionFromDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PromotionFromDate", value);
            }
        }


        /// <summary>
        /// Promotion to date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PromotionToDate
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PromotionToDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PromotionToDate", value);
            }
        }


        /// <summary>
        /// Promotion percent discount.
        /// </summary>
        [DatabaseField]
        public virtual double PromotionPercentDiscount
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("PromotionPercentDiscount"), 0d);
            }
            set
            {
                SetValue("PromotionPercentDiscount", value, 0d);
            }
        }


        /// <summary>
        /// Promotion dollar discount.
        /// </summary>
        [DatabaseField]
        public virtual int PromotionDollarDiscount
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PromotionDollarDiscount"), 0);
            }
            set
            {
                SetValue("PromotionDollarDiscount", value, 0);
            }
        }


        /// <summary>
        /// Promotion package codes.
        /// </summary>
        [DatabaseField]
        public virtual string PromotionPackageCodes
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PromotionPackageCodes"), String.Empty);
            }
            set
            {
                SetValue("PromotionPackageCodes", value, String.Empty);
            }
        }


        /// <summary>
        /// Example:.
        /// 1/6/2024.
        /// 2/6/2024.
        /// 3/7/2024-12/8/2024.
        /// </summary>
        [DatabaseField]
        public virtual string PromotionPackageDates
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PromotionPackageDates"), String.Empty);
            }
            set
            {
                SetValue("PromotionPackageDates", value, String.Empty);
            }
        }


        /// <summary>
        /// Promotion guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid PromotionGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PromotionGuid"), Guid.Empty);
            }
            set
            {
                SetValue("PromotionGuid", value);
            }
        }


        /// <summary>
        /// Promotion last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PromotionLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PromotionLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PromotionLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            PromotionInfoProvider.DeletePromotionInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            PromotionInfoProvider.SetPromotionInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected PromotionInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="PromotionInfo"/> class.
        /// </summary>
        public PromotionInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="PromotionInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public PromotionInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}