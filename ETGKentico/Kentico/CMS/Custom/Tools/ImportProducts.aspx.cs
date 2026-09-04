using Castle.Core.Internal;
using ETG.Core.PageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace CMSApp.Custom.Tools
{
    public partial class ImportProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_OnClick(object sender, EventArgs e)
        {
            

            string[] arrLine = txt.Text.Split('\n');

            foreach (var line in arrLine)
            {
                var tourLine = line.Trim();

                if (tourLine.IsNullOrEmpty())
                {
                    break;
                }

                var arr = tourLine.Split(',');

                Tour tour = new Tour();

                tour.DocumentCulture = "en-AU";
                tour.DocumentName = arr[0].Replace("||",",").Trim();
                tour.TourName = arr[0].Replace("||", ",").Trim();
                tour.TourCode = arr[1].Trim();
                tour.TourPrimaryCountry = ValidationHelper.GetGuid(arr[2], Guid.Empty);
                tour.TourHeroImage = "~/ETG/media/assets/Images/destination-hero-bg.jpg?ext=.jpg";
                var parentNode = DocumentHelper.GetDocuments().Path($"/Tour-Folder/{arr[4]}").FirstOrDefault();

                if (parentNode != null)
                {
                    tour.Insert(parentNode);
                }
            }
        }
    }
}