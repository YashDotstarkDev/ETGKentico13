using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ETG.Module.Booking.ECommerce.Payment.Models
{
    public class TravelPayPaymentResult { 
    [JsonProperty("paymentReference")]
    public string PaymentReference { get; set; }

    [JsonProperty("customerName")]
    public string CustomerName { get; set; }

    [JsonProperty("customerReference")]
    public string CustomerReference { get; set; }

    [JsonProperty("paymentStatus")]
    public string PaymentStatus { get; set; }

    [JsonProperty("baseAmount")]
    public double BaseAmount { get; set; }

    [JsonProperty("fundsToMerchant")]
    public double FundsToMerchant { get; set; }

    [JsonProperty("customerFee")]
    public double CustomerFee { get; set; }

    [JsonProperty("merchantFee")]
    public double MerchantFee { get; set; }

    [JsonProperty("paymentAmount")]
    public double PaymentAmount { get; set; }

    [JsonProperty("accountOrCardNo")]
    public string AccountOrCardNo { get; set; }

    [JsonProperty("paymentAccount")]
    public string PaymentAccount { get; set; }

    [JsonProperty("processingDate")]
    public DateTime ProcessingDate { get; set; }

    [JsonProperty("settlementDate")]
    public DateTime SettlementDate { get; set; }

    [JsonProperty("processorReference")]
    public string ProcessorReference { get; set; }

    [JsonProperty("isPaymentSettledToMerchant")]
    public bool IsPaymentSettledToMerchant { get; set; }

    [JsonProperty("failureCode")]
    public string FailureCode { get; set; }

    [JsonProperty("failureReason")]
    public string FailureReason { get; set; }

    [JsonProperty("paymentCard")]
    public string PaymentCard { get; set; }

    [JsonProperty("additionalReference")]
    public string AdditionalReference { get; set; }

    [JsonProperty("merchantName")]
    public string MerchantName { get; set; }

    [JsonProperty("merchantCode")]
    public string MerchantCode { get; set; }

    [JsonProperty("merchantUniquePaymentId")]
    public string MerchantUniquePaymentId { get; set; }

    [JsonProperty("isPaymentRetryScheduled")]
    public bool IsPaymentRetryScheduled { get; set; }

    [JsonProperty("retryScheduledOn")]
    public DateTime RetryScheduledOn { get; set; }

    [JsonProperty("retryPaymentReference")]
    public string RetryPaymentReference { get; set; }

    [JsonProperty("isPaymentRecalled")]
    public bool IsPaymentRecalled { get; set; }

    [JsonProperty("recalledOn")]
    public DateTime RecalledOn { get; set; }

    [JsonProperty("recalledReason")]
    public string RecalledReason { get; set; }

    [JsonProperty("recalledTransactionReference")]
    public string RecalledTransactionReference { get; set; }

    [JsonProperty("isPaymentRefunded")]
    public bool IsPaymentRefunded { get; set; }

    [JsonProperty("refundedOn")]
    public DateTime RefundedOn { get; set; }

    [JsonProperty("refundedReason")]
    public string RefundedReason { get; set; }

    [JsonProperty("refundedTransactionReference")]
    public string RefundedTransactionReference { get; set; }

    [JsonProperty("cardCategory")]
    public string CardCategory { get; set; }

    [JsonProperty("transactionType")]
    public double TransactionType { get; set; }

    [JsonProperty("transactionTypeDisplay")]
    public string TransactionTypeDisplay { get; set; }

    [JsonProperty("originalTransactionReference")]
    public string OriginalTransactionReference { get; set; }

    [JsonProperty("isPaymentChargeBacked")]
    public bool IsPaymentChargeBacked { get; set; }

    [JsonProperty("chargeBackedOn")]
    public DateTime ChargeBackedOn { get; set; }

    [JsonProperty("chargeBackedReason")]
    public string ChargeBackedReason { get; set; }

    [JsonProperty("chargeBackedTransactionReference")]
    public string ChargeBackedTransactionReference { get; set; }

    [JsonProperty("paymentSourceDisplay")]
    public string PaymentSourceDisplay { get; set; }

    [JsonProperty("oneOffPaymentReference")]
    public string OneOffPaymentReference { get; set; }
}
}
