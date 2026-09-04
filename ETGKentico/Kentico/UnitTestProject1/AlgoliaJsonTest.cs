using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class AlgoliaJsonTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var filePath = @"C:\Projects\ETG\Source\etg\Kentico\UnitTestProject1\TestFiles\articlewidget.json";
           
            var jsonString = File.ReadAllText(filePath);
            var json = JObject.Parse(jsonString);
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
                                    textOnly.AppendLine(variant["properties"]["text"].Value<string>());
                                }
                            }
                        }
                    }
                }
            }

            Assert.AreNotEqual(textOnly.ToString(), string.Empty);
        }
    }
}
