using CMS.DataEngine;
using Kentico.Forms.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.FormComponents
{
    public class CKEditorProperties : FormComponentProperties<string>
    {
        public CKEditorProperties()
        : base(FieldDataType.LongText)
        {
        }

        public override string DefaultValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}