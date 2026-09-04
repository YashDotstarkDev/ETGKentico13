using ETG.Data.Tour.Repositories;

namespace ETG.Data.Tour.Factories
{
    public interface ITourRepositoryFactory
    {
        ITourProductRepository GetTourRepository(string url);
        ITourProductRepository CreateTourRepository();

        //ITourProductRepository CreateCruiseRepository();
    }
}
