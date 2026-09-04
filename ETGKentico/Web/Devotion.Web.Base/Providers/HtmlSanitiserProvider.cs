using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Devotion.Web.Base.Providers
{
    public class HtmlSanitiserProvider : IHtmlSanitiserProvider
    {
        public string RemoveUnwantedTags(string input, string[] acceptTags = null)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var document = new HtmlDocument();
            document.LoadHtml(input);

            var acceptableTags = acceptTags ?? new[] { "strong", "em", "u", "li", "ol", "b", "i", "ul", "p", "br" };

            var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode ?? document.DocumentNode;


                var childNodes = node.SelectNodes("./*|./text()");
                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {
                        if (child.Name == "#text")
                        {
                            continue;
                        }

                        if (!(child.ChildNodes.All(c => acceptableTags.Contains(c.Name) || c.Name == "#text")))
                        {
                            nodes.Enqueue(child);
                            parentNode.InsertBefore(child, node);
                        }

                        if (!acceptableTags.Contains(child.Name) && child.ChildNodes.All(c => c.Name == "#text"))
                        {
                            node.RemoveChild(child);
                        }
                    }
                    //Removing empty nodes
                    if (!node.HasChildNodes)
                    {
                        parentNode.RemoveChild(node);
                    }
                }
                if (acceptableTags.Contains(node.Name) || node.Name == "#text")
                {
                    continue;
                }

                parentNode?.RemoveChild(node);
            }
            return document.DocumentNode.InnerHtml;
        }
    }
}
