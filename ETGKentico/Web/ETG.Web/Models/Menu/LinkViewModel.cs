namespace ETG.Web.Models.Menu
{
    public class LinkViewModel: IViewModel
    {
        public int Id { get; set; }

        public bool HasChildLinks { get; set; }

        public int NodeId { get; set; }

        public int NodeParentId { get; set; }

        public int NodeLevel { get; set; }

        public int NodeOrder { get; set; }

        public string Label { get; set; }

        public string NodeAliasPath { get; set; }
        public string Path { get; set; }

        public bool Active { get; set; }

        public string IconClass { get; set; }
        public string SVGIcon { get; set; }
        public string LinkGroupType { get; set; }
        public string CommonLinkCaption { get; set; }
        public string CommonLinkUrl { get; set; }
    }
}