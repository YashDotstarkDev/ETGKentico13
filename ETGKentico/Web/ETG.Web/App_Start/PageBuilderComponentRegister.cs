using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate("ETG.GenericContentTemplate", "Generic Content", null, "PageTemplates/_GenericContentTemplate")]
[assembly: RegisterPageTemplate("ETG.CampaignTemplate", "Campaign Template", null, "PageTemplates/_CampaignTemplate")]
[assembly: RegisterPageTemplate("ETG.TravelTypeDetailTemplate", "Travel Type Detail Template", null, "PageTemplates/_TravelTypeDetailTemplate")]
namespace ETG.Web
{
    public class PageBuilderComponentRegister
    {
    }
}