using System.Threading.Tasks;

namespace Devotion.Data
{
    public interface IRepositoryAsync<TModel>
    {
        Task<TModel> GetAsync(string path = "");
    }
}