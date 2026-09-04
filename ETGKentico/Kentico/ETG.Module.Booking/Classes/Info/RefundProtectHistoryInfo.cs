using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(RefundProtectHistoryInfo), RefundProtectHistoryInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="RefundProtectHistoryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class RefundProtectHistoryInfo : AbstractInfo<RefundProtectHistoryInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.refundprotecthistory";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(RefundProtectHistoryInfoProvider), OBJECT_TYPE, "ETG.RefundProtectHistory", "RefundProtectHistoryID", "RefundProtectHistoryLastModified", "RefundProtectHistoryGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("OrderID", "ecommerce.order", ObjectDependencyEnum.Required),
            },
        };


        /// <summary>
        /// Refund protect history ID.
        /// </summary>
        [DatabaseField]
        public virtual int RefundProtectHistoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("RefundProtectHistoryID"), 0);
            }
            set
            {
                SetValue("RefundProtectHistoryID", value);
            }
        }


        /// <summary>
        /// Order ID.
        /// </summary>
        [DatabaseField]
        public virtual int OrderID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("OrderID"), 0);
            }
            set
            {
                SetValue("OrderID", value);
            }
        }


        /// <summary>
        /// Transaction date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime TransactionDate
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("TransactionDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("TransactionDate", value);
            }
        }


        /// <summary>
        /// Value.
        /// </summary>
        [DatabaseField]
        public virtual double Value
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("Value"), 0d);
            }
            set
            {
                SetValue("Value", value);
            }
        }


        /// <summary>
        /// Event date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime EventDate
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("EventDate"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("EventDate", value);
            }
        }


        /// <summary>
        /// Cancelled.
        /// </summary>
        [DatabaseField]
        public virtual bool Cancelled
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("Cancelled"), false);
            }
            set
            {
                SetValue("Cancelled", value);
            }
        }


        /// <summary>
        /// Is editable.
        /// </summary>
        [DatabaseField]
        public virtual bool IsEditable
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("IsEditable"), true);
            }
            set
            {
                SetValue("IsEditable", value);
            }
        }


        /// <summary>
        /// Refund protect history guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid RefundProtectHistoryGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("RefundProtectHistoryGuid"), Guid.Empty);
            }
            set
            {
                SetValue("RefundProtectHistoryGuid", value);
            }
        }


        /// <summary>
        /// Refund protect history last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime RefundProtectHistoryLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("RefundProtectHistoryLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("RefundProtectHistoryLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            RefundProtectHistoryInfoProvider.DeleteRefundProtectHistoryInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            RefundProtectHistoryInfoProvider.SetRefundProtectHistoryInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected RefundProtectHistoryInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="RefundProtectHistoryInfo"/> class.
        /// </summary>
        public RefundProtectHistoryInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="RefundProtectHistoryInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public RefundProtectHistoryInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}