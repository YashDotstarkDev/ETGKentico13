using ETG.Data.Services;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System;
using System.Web;
using System.Web.Mvc;

namespace ETG.Web.Helpers
{
    public static class PageHelper
    {
        public static string GetPagePath(Guid guid)
        {
            var documentService = DependencyResolver.Current.GetService<IDocumentService>();

            var node = documentService.GetSingleDocument(guid);
            if (node == null)
            {
                return string.Empty;
            }
            return node.NodeAliasPath;

        }

        public static void InitializePageBuilder(HttpContextBase httpContext, int id)
        {
            httpContext.Kentico().PageBuilder().Initialize(id);
         
        }
    }
}