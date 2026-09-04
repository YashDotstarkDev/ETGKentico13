using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using CMS.Helpers;
using CMS.IO;
using CMSApp.Custom.Models;
using CsvHelper;
using ETG.Core.Forms;

namespace CMSApp.Custom.Tools
{
    public partial class ImportEnquiries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_OnClick(object sender, EventArgs e)
        {
            if (!upload.HasFile)
            {
                Response.Write("No File");
                return;
            }

            var filePath = Server.MapPath($"~/Custom/Temp/{Guid.NewGuid()}.csv");
            upload.SaveAs(filePath);

            var reader = new System.IO.StreamReader(filePath);
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<EnquiryLegacyModel>();
                foreach (var record in records)
                {
                    var item = new EnquireItem
                    {
                        Firstname = record.Firstname,
                        Lastname = record.Lastname,
                        Email = record.Email,
                        Phone = record.Phone,
                        PreferredContactMethod = record.PreferredContactMethod,
                        Message = record.Message,
                        SubscribeToNewsletter = ValidationHelper.GetBoolean(record.SubscribeToNewsletter, false),
                        Comments = record.Comments,
                        TourCode = record.TourCode,
                        TourName = record.TourName,
                        TourLink = record.TourLink,
                        Status = record.Status,
                        PreferredDestination = StoreViewToDestination(record.PreferredDestination),

                    };

                    item.Insert();

                    DateTime dt;

                    if (DateTime.TryParseExact(record.FormInserted, "dd/MM/yyyy HH:mm",
                        System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {


                        item.SetValue("FormInserted", dt);
                    }
                    item.Update();
                }
            }
        }

        private string StoreViewToDestination(string storeView)
        {
            if (string.IsNullOrWhiteSpace(storeView))
            {
                return string.Empty;
            }

            storeView = storeView.Trim();
            switch (storeView)
            {
                case "New Caledonia":
                    return storeView;
                case "French":
                    return "France";
                case "Canada Travel":
                    return "Canada";
                case "Italy Travel":
                    return "Italy";
                case "Switzerland Travel":
                    return "Switzerland";
                case "Tahiti Store View":
                    return "Tahiti";
                case "Maldives Travel":
                    return "Maldives";
                default:
                    if (storeView.IndexOf("Spain") > -1)
                    {
                        return "Spain";
                    }

                    return storeView;

            }
        }
    }
}