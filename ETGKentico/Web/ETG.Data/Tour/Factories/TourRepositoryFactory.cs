using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Internal;
using ETG.Data.Tour.Repositories;

namespace ETG.Data.Tour.Factories
{
    public class TourRepositoryFactory : ITourRepositoryFactory
    {
        private ITourProductRepository GetRepository(Type type)
        {
            var types = DependencyResolver.Current.GetServices(typeof(ITourProductRepository));
            return (ITourProductRepository) types.FirstOrDefault(a => a.GetType() == type);
        }

        public ITourProductRepository GetTourRepository(string url)
        {
            return CreateTourRepository();
            /*if (url.IsNullOrEmpty() || !url.ToLower().StartsWith("/cruises"))
            {
                return CreateTourRepository();
            }

            return CreateCruiseRepository();*/
        }

        public ITourProductRepository CreateTourRepository()
        {
            return GetRepository(typeof(TourProductRepository));
        }
        /*
        public ITourProductRepository CreateCruiseRepository()
        {
            return GetRepository(typeof(CruiseRepository));
        }*/
    }
}
