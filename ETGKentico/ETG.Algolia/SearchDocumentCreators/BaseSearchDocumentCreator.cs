using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.SiteProvider;
using ETG.Core.Extensions;
using Devotion.Web.Base.Extensions;

namespace ETG.Algolia.SearchDocumentCreators
{
    public abstract class BaseSearchDocumentCreator
    {
        protected List<Tuple<string, Func<object, object>>> FieldProcessors = new List<Tuple<string, Func<object, object>>>();
        protected string[] FieldIncludeList = { "_content" };
        protected List<string> FieldExcludeList = new List<string> { "_culture", "_index", "_type", "_site", "NodeLinkedNodeID", "_index", "DocumentID", "DocumentCategories", "DocumentCategoryIDs", "NodeAliasPath" };
        protected readonly SearchDocument searchDocument;
        protected readonly ISearchFields searchFields;
        protected bool CamelCase = false;
        public BaseSearchDocumentCreator(SearchDocument searchDocument, ISearchFields searchFields)
        {
            this.searchDocument = searchDocument;
            this.searchFields = searchFields;
        }
        private bool IsIndexField(ISearchFields searchFields, string fieldName)
        {

            var field = searchFields.Items.FirstOrDefault(a => a.FieldName == fieldName);
            if (field == null)
            {
                return false;
            }

            if (FieldIncludeList.Contains(fieldName) || field.GetFlag(AlgoliaSearchFieldFlags.SEARCHABLE) 
                                                     || field.GetFlag(AlgoliaSearchFieldFlags.RETRIEVABLE))
            {
                return true;
            }

            return false;
        }
        protected object ImgixifyProcessor(object arg)
        {
            var url = ValidationHelper.GetString(arg, string.Empty);

            if (url.IsNullOrEmpty())
            {
                return string.Empty;
            }

            
            //return $"http://{SiteContext.CurrentSite.SitePresentationDomain}{url.Replace("~", string.Empty)}";
            return $"{url.Replace("~", string.Empty).Imgixify()}";
        }
        protected virtual void UrlProcessor(JObject doc)
        {
            var nodeAliasPath = ValidationHelper.GetString(searchDocument.GetValue("NodeAliasPath"), string.Empty);
            if (string.IsNullOrWhiteSpace(nodeAliasPath))
            {
                return;
            }

            doc.Add("Url", JToken.FromObject(nodeAliasPath.ToLower()));
            
        }

        public JObject CreateDocument()
        {

            dynamic doc = new JObject();

            foreach (var name in searchDocument.Names)
            {
                if (FieldExcludeList.Contains(name) || (!searchDocument.GetStore(name) && !IsIndexField(searchFields, name)))
                {
                    continue;
                }

                var algoliaName = name;
                if (CamelCase)
                {
                    algoliaName = name[0].ToString().ToLower()[0] + name.Substring(1);
                }
                var val = searchDocument.GetValue(name);
                if (val == null)
                {
                    doc.Add(algoliaName, JToken.FromObject(""));
                    continue;
                }

                if (name.Equals("_id", StringComparison.OrdinalIgnoreCase))
                {
                    doc["objectID"] = JToken.FromObject(val);
                    continue;
                }

                if (name.Equals("SearchDescription", StringComparison.OrdinalIgnoreCase))
                {
                    doc.Add(algoliaName, JToken.FromObject(CMS.Helpers.HTMLHelper.StripTags(ValidationHelper.GetString(val, string.Empty))));
                    continue;
                }

                if (name.Equals("_content", StringComparison.OrdinalIgnoreCase))
                {
                    if (val.ToString().Trim() == "" || val.ToString().IndexOf("{") == -1)
                    {
                        continue;
                    }

                    try
                    {
                        var json = JObject.Parse(val.ToString());
                        var textOnly = new StringBuilder();
                        foreach (var area in json["editableAreas"])
                        {
                            foreach (var section in area["sections"])
                            {
                                foreach (var zone in section["zones"])
                                {
                                    foreach (var widget in zone["widgets"])
                                    {
                                        if (widget["type"].Value<string>() == "ETG.Web.Widget.HtmlTextWidget")
                                        {
                                            foreach (var variant in widget["variants"])
                                            {
                                                textOnly.Append(variant["properties"]["text"].Value<string>());
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        doc[algoliaName] = JToken.FromObject(textOnly.ToString().LimitLength(8000));
                    }
                    catch (Exception ex)
                    {
                        //EventLogProvider.LogInformation("ALG", "_CONTENT", val.ToString() +"|" + ex.Message + ex.StackTrace);
                    }

                    continue;
                }

                var processors = FieldProcessors.Where(x => x.Item1 == name);
                val = processors.Aggregate(val, (current, processor) => processor.Item2(current));

                doc.Add(algoliaName, JToken.FromObject(val));
            }

            UrlProcessor(doc);
            AddAdditionalFields(doc);
            return doc;
        }

        protected virtual void AddAdditionalFields(JObject doc)
        {

        }
    }
}
