using System.Collections.Generic;
using Castle.Core.Internal;
using ETG.Data.Models.Common;
using ETG.Web.Models.Widgets.SixColumnCallout;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("ETG.Web.Widget.SixColumnCallout", "Six column callout", typeof(SixColumnCalloutProperties), "Widgets/_SixColumnCallout")]
namespace ETG.Web.Models.Widgets.SixColumnCallout
{
    public class SixColumnCalloutProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Heading")]
        public string Heading { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 2, Label = "Label 1")]
        public string Label1 { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 3, Label = "SVG Code 1")]
        public string SVGCode1 { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Label 2")]
        public string Label2 { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 5, Label = "SVG Code 2")]
        public string SVGCode2 { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 6, Label = "Label 3")]
        public string Label3 { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 7, Label = "SVG Code 3")]
        public string SVGCode3 { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 8, Label = "Label 4")]
        public string Label4 { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 9, Label = "SVG Code 4")]
        public string SVGCode4 { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 10, Label = "Label 5")]
        public string Label5 { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 11, Label = "SVG Code 5")]
        public string SVGCode5 { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 13, Label = "Label 6")]
        public string Label6 { get; set; }
        
        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 14, Label = "SVG Code 6")]
        public string SVGCode6 { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 15, Label = "CTA Label")]
        public string CTALabel { get; set; }
        
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 16, Label = "CTA Url")]
        public string CTAUrl { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 17, Label = "Section ID")]
        public string SectionId { get; set; }


        public List<CTAIconModel> Callouts
        {
            get
            {
                var lst = new List<CTAIconModel>();

                if (!Label1.IsNullOrEmpty() && !SVGCode1.IsNullOrEmpty())
                {
                    lst.Add(new CTAIconModel
                    {
                        Label = Label1,
                        IconClass = SVGCode1
                    });
                }
                
                if (!Label2.IsNullOrEmpty() && !SVGCode2.IsNullOrEmpty())
                {
                    lst.Add(new CTAIconModel
                    {
                        Label = Label2,
                        IconClass = SVGCode2
                    });
                }
                
                if (!Label3.IsNullOrEmpty() && !SVGCode3.IsNullOrEmpty())
                {
                    lst.Add(new CTAIconModel
                    {
                        Label = Label3,
                        IconClass = SVGCode3
                    });
                }
                
                if (!Label4.IsNullOrEmpty() && !SVGCode4.IsNullOrEmpty())
                {
                    lst.Add(new CTAIconModel
                    {
                        Label = Label4,
                        IconClass = SVGCode4
                    });
                }
                
                if (!Label5.IsNullOrEmpty() && !SVGCode5.IsNullOrEmpty())
                {
                    lst.Add(new CTAIconModel
                    {
                        Label = Label5,
                        IconClass = SVGCode5
                    });
                }
                
                if (!Label6.IsNullOrEmpty() && !SVGCode6.IsNullOrEmpty())
                {
                    lst.Add(new CTAIconModel
                    {
                        Label = Label6,
                        IconClass = SVGCode6
                    });
                }

                return lst;
            }
        }
    }
}