using System.Collections.Generic;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace ETG.Web.Models.Widgets.FAQWidget
{
    public class FAQWidgetProperties : IWidgetProperties
    {
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Label = "Hide View All", DefaultValue = false)]
        public bool HideViewAll { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, DefaultValue = 5)]
        public int? Count { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER)]
        public string Heading { get; set; }

        // Assigns a selector component to the 'PagePaths' property
        [EditingComponent(PathSelector.IDENTIFIER)]
        // Returns a list of path selector items (page paths)
        public IList<PathSelectorItem> Path { get; set; }
    }
}