using Castle.Core.Internal;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Search;
using CMS.SiteProvider;
using CMS.UIControls;
using CMSApp.Custom.Models;
using CommonServiceLocator;
using CsvHelper;
using ETG.Algolia.Tasks;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Classes.Providers;
using ETG.Core.CustomTables;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Search;
using ETG.Module.Booking.CustomTables;
using ETG.Module.Booking.Models.HotelBuilder;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ETG.Module.Booking.HotelBuilder.Models;

namespace CMSApp.CMSModules.ETGBooking.PageTemplates
{
    
    public partial class HotelPDFBuilder : CMSPage
    {
        protected string HotelSelectStyle
        {
            get;
            set;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HotelSelectStyle = "style=\"display:none\"";
            }
            else
            {
                
            }
        }

        private bool ValidateTourCode(string tourCode)
        {
            return TourProvider.GetTours().WhereEquals(nameof(Tour.TourCode), tourCode).Any();
        }
        private string ErrorText(string error)
        {
            return $"<span style=\"color:red\">{error}</span>";
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            
            var requestParam = new HotelBuilderRequest
            {
                TourCode = txtTourCode.Text,
                BookingNumber = txtBookingNumber.Text,
                HotelIds = hidHotelIds.Value.Split(',').ToList(),
                Nights = hidNights.Value.Split(',').ToList(),
                TravelStartDate = txtTravelStartDate.Text

            };

            var request =  WebRequest.Create($"{SiteContext.CurrentSite.SitePresentationURL}/printwithhotel");

            var postData = JsonConvert.SerializeObject(requestParam);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "text/json";
            request.ContentLength = byteArray.Length;
            request.Method = "POST";

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the request stream.
            var response = request.GetResponse();

            var stream = response.GetResponseStream();
            var memoryStream = new MemoryStream();

            byte[] buffer = new byte[1024];
            int byteCount;
            do
            {
                byteCount = stream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, byteCount);
            } while (byteCount > 0);


            byte[] bytesInStream = memoryStream.ToArray(); // simpler way of converting to array
            memoryStream.Close();

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AppendHeader("content-disposition", $"inline;filename={txtBookingNumber.Text}.pdf");
            Response.BinaryWrite(bytesInStream);
            Response.End();
        }

        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            HotelSelectStyle = "style=\"display:none\"";
            if (!ValidateTourCode(txtTourCode.Text))
            {
                litCreateMessage.Text = ErrorText("Invalid Tour code");
                return;
            }

            DateTime dt;
            if (!DateTime.TryParseExact(txtTravelStartDate.Text, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                litCreateMessage.Text = ErrorText("Invalid date");
                return;
            }

            litCreateMessage.Text = "";
            btnNext.Visible = false;
            txtBookingNumber.Enabled = false;
            txtTourCode.Enabled = false;
            txtTravelStartDate.Enabled = false;
            HotelSelectStyle = "style=\"display:block\"";
        }
    }
}