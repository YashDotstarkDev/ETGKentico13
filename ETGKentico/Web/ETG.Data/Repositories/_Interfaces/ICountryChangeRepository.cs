using ETG.Data.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Repositories
{
    public interface ICountryChangeRepository
    {
        Task<CountryInforModel> GetCountryInfoAsync(string IpAddress);
    }
}
