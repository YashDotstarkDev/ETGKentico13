using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ETG.Data.Serialization
{
    public class XmlSerializer
    {

        public XmlSerializer()
        {

        }

        public string Serialize<T>(T data)
        {
            StringWriter stringWriter = (StringWriter)new StringWriter();
            this.SerializeToStream<T>(data, (Func<XmlWriterSettings, XmlWriter>)(settings => XmlWriter.Create((TextWriter)stringWriter, settings)));
            return stringWriter.ToString();
        }

        public void SerializeToStream<T>(T data, Stream stream)
        {
            this.SerializeToStream<T>(data, (Func<XmlWriterSettings, XmlWriter>)(settings => XmlWriter.Create(stream, settings)));
        }

        private void SerializeToStream<T>(T data, Func<XmlWriterSettings, XmlWriter> createXmlWriter)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "http://www.sitemaps.org/schemas/sitemap/0.9");

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                NamespaceHandling = NamespaceHandling.OmitDuplicates
            };
            using (XmlWriter xmlWriter = createXmlWriter(xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, (object)data, namespaces);
            }
        }
    }
}
