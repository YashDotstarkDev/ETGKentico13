using ETG.Algolia.Classes;

namespace ETG.Algolia
{
    public interface ISearchTaskEngine
    {
        void ProcessAlgoliaSearchTask(SearchTaskAlgoliaInfo task);
    }
}