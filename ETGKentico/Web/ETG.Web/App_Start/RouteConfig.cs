using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CMS.DocumentEngine;
using ETG.Core.PageTypes;
using ETG.Web;
using Kentico.Web.Mvc;

namespace ETG
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Maps routes to Kentico HTTP handlers and features enabled in ApplicationConfig.cs
            // Always map the Kentico routes before adding other routes. Issues may occur if Kentico URLs are matched by a general route, for example images might not be displayed on pages
            routes.Kentico().MapRoutes();

            //Homepage
            routes.MapRoute("Home", "",
                new {controller = "Home", action = "Index"});
            
            // routes.MapRoute("Travel Type Detail", "{path}",
            //     new { controller = "TravelType", action = "Index" },
            //     new { path = new PageTypeRouteConstraint(TravelTypeDetail.CLASS_NAME)});
            
            // RegisterAlternativeRoutes(routes, TravelTypeDetail.CLASS_NAME, "TravelType");

            routes.MapRoute("Articles All", "articles",
                new {controller = "Article", action = "Landing"});

            routes.MapRoute("Articles Europe", "articles/europe",
                new {controller = "Article", action = "Landing"});

            routes.MapRoute("Articles Islands", "articles/islands",
                new {controller = "Article", action = "Landing"});

            routes.MapRoute("Articles Listicles", "articles/listicles",
                new {controller = "Article", action = "Landing"});

            routes.MapRoute("Articles North America", "articles/north-america",
                new {controller = "Article", action = "Landing"});

            routes.MapRoute("Articles Podcasts", "articles/podcasts",
                new {controller = "Article", action = "Landing"});

            routes.MapRoute("Article Page", "articles/{alias}",
                new {controller = "Article", action = "Index"});

            routes.MapRoute("Tour Print Version", "tourprint/{alias}",
                new {controller = "Tour", action = "Print"});

            routes.MapRoute("Tour With Hotel Print Version", "tourprintwithhotels/{alias}/{bookingnumber}",
                new {controller = "Tour", action = "PrintWithHotels"});

            routes.MapRoute("Tour Download", "pdf/{tourCode}",
                new {controller = "Tour", action = "GeneratePdf"});

            routes.MapRoute("Print PDF", "print",
                new {controller = "Tour", action = "GenerateUrlPDF"});

            routes.MapRoute("Print Hotel Builder", "printwithhotel",
                new {controller = "Tour", action = "GeneratePDFWithHotels"});

            routes.MapRoute("Tours Enquire Thank you", "enquire/thank-you",
                new {controller = "Default", action = "Index"});

            routes.MapRoute("Tours Enquire", "enquire/{tourcode}",
                new {controller = "Tour", action = "Enquire"});

            routes.MapRoute("Destination", "destinations/{alias}",
                new {controller = "Destination", action = "Destination" });
            
            routes.MapRoute("Destination Enquire", "destinations/enquire/{alias}",
                new {controller = "Destination", action = "Enquire"});

            routes.MapRoute("Destination Region", "destinations/{region}/{alias}",
                new {controller = "Destination", action = "Destination" });

            routes.MapRoute("Destination Landing", "destinations",
                new {controller = "Destination", action = "Landing"});

            routes.MapRoute("Experience", "experiences/{alias}",
                new {controller = "Experience", action = "Index"});
            
            routes.MapRoute("Experience Enquire", "experiences/enquire/{alias}",
                new {controller = "Experience", action = "Enquire"});

            routes.MapRoute("Experience Landing", "experiences",
                new {controller = "Experience", action = "Landing"});

            routes.MapRoute("Destination Experts", "destination-experts/{alias}",
                new {controller = "DestinationExpertTeam", action = "Index"});

            routes.MapRoute("Destination Experts Landing", "destination-experts",
                new {controller = "DestinationExpertTeam", action = "Landing"});

            routes.MapRoute("Enquire Generic", "enquire",
                new {controller = "Enquire", action = "Index"});

            routes.MapRoute("Careers", "careers",
                new {controller = "Career", action = "Index"});

            routes.MapRoute("Careers Apply", "careers/apply",
                new {controller = "Career", action = "Apply", rolealias = UrlParameter.Optional});

            routes.MapRoute("Careers Role", "careers/{alias}",
                new {controller = "Career", action = "Role"});

            routes.MapRoute("Careers Role Apply", "careers/{rolealias}/apply",
                new {controller = "Career", action = "Apply"});

            routes.MapRoute("Brochures", "brochures",
                new {controller = "Brochure", action = "Index"});

            routes.MapRoute("Brochures Order", "brochures/order",
                new {controller = "Brochure", action = "Order"});

            routes.MapRoute("Brochures View", "brochures/{alias}",
                new {controller = "Brochure", action = "View"});

            routes.MapRoute("Contact Us", "contact-us",
                new {controller = "ContactUs", action = "Index"});
            
            routes.MapRoute("FAQs page", "faqs",
                new {controller = "FAQsPage", action = "Index"});

            routes.MapRoute("Agent Portal", "agents",
                new {controller = "AgentsPortal", action = "Index"});

            routes.MapRoute("Agent Incentive", "agents/incentives/{alias}",
                new {controller = "AgentsPortal", action = "Incentive"});

            /*routes.MapRoute("Cruise", "cruises/{alias}",
                new { controller = "Cruise", action = "Index" });
                */
            routes.MapRoute("Travel deals", "travel-deals",
                new {controller = "TravelDeals", action = "Index"});

            /*routes.MapRoute("Search", "search",
                new {controller = "Search", action = "Index"});*/
            
            routes.MapRoute("Package Search", "search",
                new {controller = "Search", action = "PackageSearch"});

            routes.MapRoute("Newsletter Unsubscribe", "newsletter/unsubscribe",
                new {controller = "Newsletter", action = "Unsubscribe"});

            routes.MapRoute("Sitemap", "sitemap.xml",
                new {controller = "SiteMap", action = "Index"});

            routes.MapRoute("GetFileFromBlob", "getfile",
                new {controller = "File", action = "Index"});

            routes.MapRoute("Robot", "robots.txt",
                new {controller = "Robot", action = "Index"});

            routes.MapRoute("500", "500",
                new {controller = "Error", action = "PageError"});

            routes.MapRoute("404", "404",
                new {controller = "Error", action = "PageNotFound"});

            routes.MapRoute("Timer", "timer",
                new {controller = "Default", action = "Timer"});

            routes.MapRoute("PDF Test", "pdf",
                new {controller = "PDF", action = "Index"});

            //routes.MapRoute("Booking Test", "booking/{tourcode}/{skuid}",
            //    new { controller = "TourBooking", action = "BookingTest" });

            routes.MapRoute("Checkout Form", "booking/checkout",
                new {controller = "TourBooking", action = "CheckoutForm"});

            routes.MapRoute("Booking Success", "booking/success",
                new {controller = "TourBooking", action = "BookingSuccess"});

            routes.MapRoute("Quote Success", "booking/quote-success",
                new {controller = "TourBooking", action = "QuoteSuccess"});

            routes.MapRoute("Travel Pay Booking Redirect", "booking/process",
                new {controller = "TourBooking", action = "BookingProcess"});

            routes.MapRoute("Quote PDF Download", "booking/downloadquote",
                new {controller = "TourBooking", action = "DownloadQuote"});

            routes.MapRoute("Quote Invalid", "quote-invalid",
                new {controller = "TourBooking", action = "QuoteInvalid"});

            routes.MapRoute("Payment page", "payments",
                new {controller = "TourBooking", action = "PaymentPage"});
            
            routes.MapRoute("Pay Later", "payments/otherpaymentsuccess",
                new {controller = "TourBooking", action = "OtherPaymentSuccess"});

            routes.MapRoute("Travel Pay Pay Now Redirect", "payments/process",
                new {controller = "TourBooking", action = "PaymentProcess"});

            routes.MapRoute("Payment Success", "payment/success",
                new {controller = "TourBooking", action = "PaymentSuccess"});

            routes.MapRoute("Payment Failed", "payment/failed",
                new {controller = "TourBooking", action = "PaymentFailed"});

            routes.MapRoute("Refundable", "refundable",
                new {controller = "TourBooking", action = "Refundable"});

            routes.MapRoute("Quote Checkout Form", "booking/quote",
                new {controller = "TourBooking", action = "QuoteCheckoutForm"});

            routes.MapRoute("Apply Refund", "apply-refund",
                new {controller = "TourBooking", action = "ApplyRefund"});

            routes.MapRoute("ChangeCurrency", "change-currency",
                new {controller = "MasterPage", action = "ChangeCurrency"});
            
            // Partials
            routes.MapRoute(
                "Partials",
                "_child-action-only/{controller}/{action}");


            routes.MapRoute("Campaigns", "landing/{*path}",
                new {controller = "Default", action = "CampaignLanding"});


            routes.MapRoute("Generic Pages", "generic-contents/{*path}",
                new {controller = "Default", action = "GenericContent"});
            


            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new {controller = "Default", action = "Index"}
            );
            

        }
        
        private static void RegisterAlternativeRoutes(RouteCollection routes, string className, string controllerName)
        {
            var pages = DocumentHelper.GetDocuments().AllCultures()
                .WhereEquals(nameof(TreeNode.ClassName), className)
                .Columns(nameof(TreeNode.NodeName), nameof(TreeNode.NodeAliasPath));

            if (pages != null && pages.Any())
            {
                foreach (var page in pages.Where(x => !string.IsNullOrWhiteSpace(x.NodeAliasPath)))
                {
                    var path = page.NodeAliasPath.TrimStart('/');
                    routes.MapRoute(
                        page.NodeName,
                        path,
                        new { controller = controllerName, action = "Index", path });
                }
            }
        }
    }
}