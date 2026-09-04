using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Tour.Services;
using ETG.Web.Models.Widgets.ProductsWidget;
using ETG.Web.Tour.Models;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Devotion.Web.Base.Extensions;
using ETG.Data.Tour.Models;

namespace ETG.Web.Models.Widgets.ProductsWidget
{
    public class ProductsWidgetProperties : IWidgetProperties
    {

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Heading")]
        public string Heading { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Package codes", Tooltip = "Separated by ,")]
        public string TourCodes { get; set; }
        [EditingComponent(DropDownComponent.IDENTIFIER, Order = 2, Label = "Tour type")]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), ";Select\r\nPackageTour;Package Tour\r\nsmallgrouptours;Small group  tours\r\nescortedgrouptours;Escorted group tours\r\nprivatetours;Private tours\r\nindependenttours;Independent tours\r\ndaytours;Day tours")]
        public string TourType { get; set; }

        [EditingComponent(DropDownComponent.IDENTIFIER, Order = 3, Label = "Cruise type")]
        [EditingComponentProperty(nameof(DropDownProperties.DataSource), ";Select\r\n1;Self-Drive\r\n2;Barge\r\n3;River\r\n4;Ocean")]
        public string CruiseType { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 4, Label = "View all url")]
        public string ViewAllUrl { get; set; }

        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 5, Label = "Hide View All Button")]
        public bool HideViewAll { get; set; }

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 5, Label = "Section ID")]
        public string SectionId { get; set; }
    }
}