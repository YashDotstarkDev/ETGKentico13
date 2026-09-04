namespace Devotion.Web.Base.ViewModels
{
    public abstract class PageViewModel<TViewModel> : AbstractViewModel<TViewModel>, IMappable where TViewModel : AbstractViewModel<TViewModel>
    {
        public string PageTitle { get; set; }

        public string PageDescription { get; set; }

        public string PageKeywords { get; set; }

    }
}