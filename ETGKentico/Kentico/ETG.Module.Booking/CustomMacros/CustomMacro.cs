using CMS;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.MacroEngine;
using ETG.Module.Booking.CustomMacros;
using ETG.Module.Booking.Extensions;
using System;
using ETG.Data.Tour;

// Makes all methods in the 'CustomMacroMethods' container class available for string objects
[assembly: RegisterExtension(typeof(CustomMacro), typeof(string))]
// Registers methods from the 'CustomMacroMethods' container into the "String" macro namespace
[assembly: RegisterExtension(typeof(CustomMacro), typeof(StringNamespace))]
namespace ETG.Module.Booking.CustomMacros
{
    public class CustomMacro : MacroMethodContainer
    {
        [MacroMethod(typeof(string), "Return Booking Breakdown", 1)]
        [MacroMethodParam(0, "param1", typeof(OrderInfo), "Order")]

        public static object BookingBreakdownEmailHtml(EvaluationContext context, params object[] parameters)
        {
            var order = (OrderInfo) parameters[0];
            if (order == null)
            {
                EventLogProvider.LogInformation("Macro", "ordernull");
                return string.Empty;
            }

            
            return order.BookingBreakdownEmailHtml(new CurrentCurrencyPricing("AUD", 1));
     
        }

        [MacroMethod(typeof(string), "Return Booking Package info", 1)]
        [MacroMethodParam(0, "param1", typeof(OrderInfo), "Order")]
        public static object BookingTourInfoEmailHtml(EvaluationContext context, params object[] parameters)
        {
            var order = (OrderInfo)parameters[0];
            if (order == null)
            {   
                return string.Empty;
            }

            return order.BookingTourInfoEmailHtml();
     
        }


        [MacroMethod(typeof(string), "Return Booking Customer Info", 1)]
        [MacroMethodParam(0, "param1", typeof(OrderInfo), "Order")]
        public static object BookingCustomerInfoEmailHtml(EvaluationContext context, params object[] parameters)
        {
            var order = (OrderInfo)parameters[0];
            if (order == null)
            {
                return string.Empty;
            }
            return order.BookingCustomerInfoEmailHtml();
         
        }

    }
}
