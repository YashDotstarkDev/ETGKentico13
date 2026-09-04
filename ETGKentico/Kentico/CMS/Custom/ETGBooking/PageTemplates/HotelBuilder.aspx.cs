using CMS.SiteProvider;
using CMS.UIControls;
using ETG.Core.PageTypes.Providers;
using ETG.Module.Booking.HotelBuilder.Models;
using ETG.Module.Booking.Models.HotelBuilder;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CMSApp.CMSModules.ETGBooking.PageTemplates
{

    public partial class HotelBuilder : CMSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var hotels = HotelProvider.GetHotels().OnCurrentSite().Select(
                    hotel=> new HotelEntity
                    {
                        HotelID = hotel.HotelID,
                        HotelName = hotel.HotelName,
                        ParentName = GetParentName(hotel.NodeAliasPath)
                    }).ToList();
                repHotels.DataSource = hotels.OrderBy(a=>a.ParentName).ThenBy(a=>a.HotelName);
                repHotels.DataBind();
            }
        }

        private string GetParentName(string nodeAliasPath)
        {
            var index = nodeAliasPath.LastIndexOf("/");

            return nodeAliasPath.Substring(0, index).Replace("-", " ").Replace("/Hotels/", string.Empty).Replace("/", "&nbsp;>&nbsp;");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            var requestParam = new HotelBuilderRequest
            {
                TourCode = txtTourCode.Text,
                BookingNumber = txtBookingNumber.Text,
                HotelIds = hidHotelIds.Value.Split(',').ToList()
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
    }
}