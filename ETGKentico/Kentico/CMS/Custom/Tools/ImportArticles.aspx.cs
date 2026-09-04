using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using CMS.DocumentEngine;
using CMS.Helpers;
using ETG.Core.PageTypes;

namespace CMSApp.Custom.Tools
{
    public partial class ImportArticles : System.Web.UI.Page
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

                Article article = new Article();

                article.DocumentCulture = "en-AU";
                article.DocumentName = FormatTitle(arr[1]);
                article.ArticleDate = DateTime.Now.Date;
                article.ArticleTitle = FormatTitle(arr[1]);
                article.ArticleDestinations = ddlDest.SelectedValue;
                article.ArticleHeroImage = "~/ETG/media/assets/Images/article-hero.jpg?ext=.jpg";
                var parentNode = DocumentHelper.GetDocuments().Path($"/Articles-Folder/{ddlDest.SelectedItem.Text.Replace(" ", "-")}").FirstOrDefault();

                if (parentNode != null)
                {
                    article.Insert(parentNode);
                }
            }
        }

        private string FormatTitle(string v)
        {
            v = v.Replace("-", " ").Trim('/');
            v = v.Substring(0, 1).ToUpper() + v.Substring(1);

            return v;
        }
    }
}