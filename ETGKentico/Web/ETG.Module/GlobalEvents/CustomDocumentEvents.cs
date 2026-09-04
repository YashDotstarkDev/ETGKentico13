using CMS;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Search;
using CommonServiceLocator;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Repositories;
using ETG.Core.Search;
using ETG.Module.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

[assembly: RegisterModule(typeof(CustomDocumentEvents))]
public class CustomDocumentEvents : Module
{
    // Module class constructor, the system registers the module under the name "CustomInit"
    public CustomDocumentEvents()
        : base("DocumentEvents")
    {
    }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit()
    {
        base.OnInit();

        // Assigns custom handlers to events
        DocumentEvents.Insert.After += Document_Insert_After;
        DocumentEvents.Update.After += Document_Update_After;
        DocumentEvents.Delete.After += Document_Delete_After;
        DocumentEvents.Insert.Before += Document_Insert_Before;
        DocumentEvents.Update.Before += Document_Update_Before;
    }

    
    private void Document_Insert_Before(object sender, DocumentEventArgs e)
    {
        if (e.Node.ClassName == RoomUpgrade.CLASS_NAME || e.Node.ClassName == OptionalExtras.CLASS_NAME)
        {
            if (e.Node.ClassName == OptionalExtras.CLASS_NAME)
            {
                e.Node.DocumentName =
                    $"{e.Node.GetValue("OptionalExtrasNumber")} - {e.Node.GetValue("OptionalExtrasTitle")}";
            }else if (e.Node.ClassName == RoomUpgrade.CLASS_NAME)
            {
                e.Node.DocumentName =
                    $"{e.Node.GetValue("RoomUpgradeNumber")} - {e.Node.GetValue("RoomUpgradeTitle")}";
            }
        }
    }
    private void Document_Update_Before(object sender, DocumentEventArgs e)
    {
        if (e.Node.ClassName == RoomUpgrade.CLASS_NAME || e.Node.ClassName == OptionalExtras.CLASS_NAME)
        {
            if (e.Node.ClassName == OptionalExtras.CLASS_NAME)
            {
                e.Node.DocumentName =
                    $"{e.Node.GetValue("OptionalExtrasNumber")} - {e.Node.GetValue("OptionalExtrasTitle")}";
            }
            else if (e.Node.ClassName == RoomUpgrade.CLASS_NAME)
            {
                e.Node.DocumentName =
                    $"{e.Node.GetValue("RoomUpgradeNumber")} - {e.Node.GetValue("RoomUpgradeTitle")}";
            }
        }
    }

    private void Document_Delete_After(object sender, DocumentEventArgs e)
    {
        if (e.Node.ClassName == TourPricing.CLASS_NAME)
        {
            var tourAliasPath = PathHelper.Get2LevelUpAliasPath(e.Node.NodeAliasPath);

            if (tourAliasPath != string.Empty)
            {
                var treeNode = DocumentHelper.GetDocuments().OnCurrentSite().Path(tourAliasPath).FirstOrDefault();

                if (treeNode != null)
                {
                    AddToSearchTask(treeNode, SearchTaskTypeEnum.Update);
                }
            }
            return;
        }
        else if (e.Node.ClassName == DiscountAndOffer.CLASS_NAME)
        {
            CreateSearchTaskForDiscountUpdates(e.Node.NodeGUID, SearchTaskTypeEnum.Update, "cms.document");

            return;
        }
        AddToSearchTask(e.Node, SearchTaskTypeEnum.Delete, e.Node.ClassName);
    }

    private void AddToSearchTask(TreeNode treeNode, SearchTaskTypeEnum taskType, string objectType = "cms.document")
    {

        if (treeNode.ClassName == TourPricing.CLASS_NAME)
        {

            var tourAliasPath = PathHelper.Get2LevelUpAliasPath(treeNode.NodeAliasPath);

            if (tourAliasPath != string.Empty)
            {
                treeNode = DocumentHelper.GetDocuments().OnCurrentSite().Path(tourAliasPath).FirstOrDefault();

            }
        }
        else if (treeNode.ClassName == DiscountAndOffer.CLASS_NAME)
        {
            CreateSearchTaskForDiscountUpdates(treeNode.NodeGUID, taskType, objectType);

            return;
        }

        if (treeNode.ClassName == Tour.CLASS_NAME)
        {
            CreateSearchTask(treeNode, taskType, objectType);
            
        }
    }

    private void CreateSearchTaskForDiscountUpdates(Guid discountNodeGuid, SearchTaskTypeEnum taskType, string objectType)
    {
        var tours = TourProvider.GetTours().WhereEquals(nameof(Tour.TourDiscountAndOffer), discountNodeGuid).ToList();

        if (tours == null || tours.Count == 0)
        {
            return;
        }

        foreach (var tour in tours)
        {
            CreateSearchTask(tour, taskType, objectType);
        }
    }

    private void CreateSearchTask(TreeNode treeNode, SearchTaskTypeEnum taskType, string objectType = "cms.document")
    {
        var etgSearchTaskCreator = ServiceLocator.Current.GetInstance<IETGSearchTaskCreator>();
        etgSearchTaskCreator.CreateSearchTask(treeNode, taskType, objectType);
    }
    private void Document_Update_After(object sender, DocumentEventArgs e)
    {

        AddToSearchTask(e.Node, SearchTaskTypeEnum.Update);
    }

    private bool CreateTourSubFolders => ValidationHelper.GetBoolean(ConfigurationManager.AppSettings["CreateTourSubFolders"], false);

    private void Document_Insert_After(object sender, DocumentEventArgs e)
    {
        if (CreateTourSubFolders)
        {
            if (e.Node.ClassName == Tour.CLASS_NAME)
            {
                var treeRepository = new ETGTreeNodeRepository(new KenticoSiteContext());

                treeRepository.CreateNewPage("ETG.ImageFolder", "Hero images", e.Node, "cms.blankmasterpage");
                treeRepository.CreateNewPage("ETG.TourHighlightFolder", "Highlights", e.Node, "cms.blankmasterpage");
                treeRepository.CreateNewPage("ETG.InclusionFolder", "Inclusions", e.Node, "cms.blankmasterpage");
                treeRepository.CreateNewPage("ETG.ItineraryFolder", "Itineraries", e.Node, "cms.blankmasterpage");
                treeRepository.CreateNewPage("ETG.TourPricingFolder", "Pricing", e.Node, "cms.blankmasterpage");

            }
            else if (e.Node.ClassName == Destination.CLASS_NAME)
            {
                var treeRepository = new ETGTreeNodeRepository(new KenticoSiteContext());

                treeRepository.CreateNewPage("ETG.ImageFolder", "Hero images", e.Node, "cms.blankmasterpage");
                treeRepository.CreateNewPage("ETG.AccordionItemFolder", "Helpful information", e.Node,
                    "cms.blankmasterpage");

            }
        }

        AddToSearchTask(e.Node, SearchTaskTypeEnum.Update);
    }

}