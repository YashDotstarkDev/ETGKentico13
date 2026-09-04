using ETG.Web.Models.Widgets.IconsContent;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("ETG.Web.Widget.IconsContent", "Icons content", typeof(IconsContentProperties), "Widgets/_IconsContent")]
namespace ETG.Web.Models.Widgets.IconsContent
{
    public class IconsContentProperties : IWidgetProperties
    {
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Heading")]
        public string Heading { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Icon 1")]
        public string Stat1Icon { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 2, Label = "Title 1")]
        public string Stat1Title { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 3, Label = "Description 1")]
        public string Stat1Description { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "Icon 2")]
        public string Stat2Icon { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 5, Label = "Title 2")]
        public string Stat2Title { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 6, Label = "Description 2")]
        public string Stat2Description { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 7, Label = "Icon 3")]
        public string Stat3Icon { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 8, Label = "Title 3")]
        public string Stat3Title { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 9, Label = "Description 3")]
        public string Stat3Description { get; set; }
        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 10, Label = "Icon 4")]
        public string Stat4Icon { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 11, Label = "Title 4")]
        public string Stat4Title { get; set; }

        [EditingComponent(TextAreaComponent.IDENTIFIER, Order = 12, Label = "Description 4")]
        public string Stat4Description { get; set; }
    }
}