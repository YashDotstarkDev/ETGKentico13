using CMS.DocumentEngine;
using CMS.Membership;
using CMS.PortalEngine;
using ETG.Core.Kentico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Core.Repositories
{
    public class ETGTreeNodeRepository
    {
        TreeProvider _tree;
        ISiteContext _siteContext;
        public ETGTreeNodeRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
            _tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        }
        public TreeNode GetNode(string nodeAliasPath)
        {
            //check if folder already exist
            TreeNode node = _tree.SelectNodes()
                    .Path(nodeAliasPath)
                    .OnCurrentSite()
                    .Culture("en-au")
                    .FirstOrDefault();

            return node;
        }

        public IEnumerable<TreeNode> GetNodes(string nodeAliasPath, string className, string columns)
        {
            return DocumentHelper.GetDocuments(className).Path(nodeAliasPath).OnCurrentSite().Culture("en-au").Columns(columns);

        }

        public void DeleteChildNodes(string path, string classname)
        {
            if (classname == string.Empty || path == "%")
            {
                return;
            }

            //check if folder already exist
            var nodeList = _tree.SelectNodes(classname)
                            .Path(path)
                            .OnCurrentSite()
                            .Culture(_siteContext.CurrentCultureCode)
                            .ToList();

            foreach (TreeNode node in nodeList)
            {
                node.Delete();
            }

        }

        public TreeNode CreateNewPage(string className, string documentName, TreeNode parentNode, string templateName)
        {
            if (parentNode == null)
            {
                return null;
            }

            TreeNode newPage = TreeNode.New(className, _tree);

            PageTemplateInfo emptyTemplate = PageTemplateInfoProvider.GetPageTemplateInfo(templateName);
            if (emptyTemplate != null)
            {
                //newPage.DocumentPageTemplateID = emptyTemplate.PageTemplateId;
            }

            // Sets the properties of the new page
            newPage.DocumentName = documentName;
            newPage.DocumentCulture = _siteContext.CurrentCultureCode;

            // Inserts the new page as a child of the parent page
            newPage.Insert(parentNode);

            return newPage;
        }
    }
}
