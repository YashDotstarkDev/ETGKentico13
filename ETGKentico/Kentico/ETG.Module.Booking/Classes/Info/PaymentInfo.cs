using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

[assembly: RegisterObjectType(typeof(PaymentInfo), PaymentInfo.OBJECT_TYPE)]

namespace ETG.Module.Booking.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="PaymentInfo"/>.
    /// </summary>
    [Serializable]
    public partial class PaymentInfo : AbstractInfo<PaymentInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.payment";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(PaymentInfoProvider), OBJECT_TYPE, "ETG.Payment", "PaymentID", "PaymentLastModified", "PaymentGuid", null, null, null, null, null, null)
        {
            ModuleName = "ETGBooking",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Payment ID.
        /// </summary>
        [DatabaseField]
        public virtual int PaymentID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("PaymentID"), 0);
            }
            set
            {
                SetValue("PaymentID", value);
            }
        }


        /// <summary>
        /// Payment merchant unique payment id.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentMerchantUniquePaymentId
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentMerchantUniquePaymentId"), String.Empty);
            }
            set
            {
                SetValue("PaymentMerchantUniquePaymentId", value, String.Empty);
            }
        }


        /// <summary>
        /// Payment first name.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentFirstName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentFirstName"), String.Empty);
            }
            set
            {
                SetValue("PaymentFirstName", value);
            }
        }


        /// <summary>
        /// Payment last name.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentLastName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentLastName"), String.Empty);
            }
            set
            {
                SetValue("PaymentLastName", value);
            }
        }


        /// <summary>
        /// Payment email.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentEmail
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentEmail"), String.Empty);
            }
            set
            {
                SetValue("PaymentEmail", value);
            }
        }


        /// <summary>
        /// Payment contact number.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentContactNumber
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentContactNumber"), String.Empty);
            }
            set
            {
                SetValue("PaymentContactNumber", value);
            }
        }


        /// <summary>
        /// Payment invoice reference.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentInvoiceReference
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentInvoiceReference"), String.Empty);
            }
            set
            {
                SetValue("PaymentInvoiceReference", value);
            }
        }


        /// <summary>
        /// Payment amount.
        /// </summary>
        [DatabaseField]
        public virtual double PaymentAmount
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("PaymentAmount"), 0d);
            }
            set
            {
                SetValue("PaymentAmount", value, 0d);
            }
        }


        /// <summary>
        /// Payment amount paid.
        /// </summary>
        [DatabaseField]
        public virtual double PaymentAmountPaid
        {
            get
            {
                return ValidationHelper.GetDouble(GetValue("PaymentAmountPaid"), 0d);
            }
            set
            {
                SetValue("PaymentAmountPaid", value, 0d);
            }
        }


        /// <summary>
        /// Payment reference.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentReference
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentReference"), String.Empty);
            }
            set
            {
                SetValue("PaymentReference", value, String.Empty);
            }
        }


        /// <summary>
        /// Payment additional info.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentAdditionalInfo
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentAdditionalInfo"), String.Empty);
            }
            set
            {
                SetValue("PaymentAdditionalInfo", value, String.Empty);
            }
        }


        /// <summary>
        /// Payment details.
        /// </summary>
        [DatabaseField]
        public virtual string PaymentDetails
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PaymentDetails"), String.Empty);
            }
            set
            {
                SetValue("PaymentDetails", value, String.Empty);
            }
        }


        /// <summary>
        /// Payment guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid PaymentGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("PaymentGuid"), Guid.Empty);
            }
            set
            {
                SetValue("PaymentGuid", value);
            }
        }


        /// <summary>
        /// Payment created.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PaymentCreated
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PaymentCreated"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PaymentCreated", value, DateTimeHelper.ZERO_TIME);
            }
        }


        /// <summary>
        /// Payment last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime PaymentLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("PaymentLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("PaymentLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            PaymentInfoProvider.DeletePaymentInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            PaymentInfoProvider.SetPaymentInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected PaymentInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="PaymentInfo"/> class.
        /// </summary>
        public PaymentInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="PaymentInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public PaymentInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}