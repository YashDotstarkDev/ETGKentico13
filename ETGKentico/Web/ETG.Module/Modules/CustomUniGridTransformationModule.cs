using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.UIControls;

[assembly: RegisterModule(typeof(CustomUniGridTransformationModule))]
public class CustomUniGridTransformationModule : Module
    {
        public CustomUniGridTransformationModule()
            : base("CustomUniGridTransformationModule")
        {
        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();
            UniGridTransformations
            .Global.RegisterTransformation("#money", MoneyFormat);

        }
        private static object MoneyFormat(object parameter)
        {
            return "$" + ValidationHelper.GetDouble(parameter, 0).ToString("#,###.##");
        }

}