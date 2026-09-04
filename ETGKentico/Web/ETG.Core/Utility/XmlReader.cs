using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CMS.Helpers;

namespace ETG.Core.Utility
{
    public class XmlReader : IXmlReader
    {
        private XmlDocument document;
        
        public void LoadXml(string xml)
        {
            document = new XmlDocument();
            document.LoadXml(xml);
        }

        public string GetText(string path)
        {
            if (document == null)
            {
                throw new Exception("XML Document is null. LoadXml needed to be invoked first.");
            }

            var xmlNode = document.SelectSingleNode(path);
            return xmlNode?.InnerText;
        }
    }
}
