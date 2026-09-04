namespace Devotion.Data
{
    public interface IRepository<out TModel>
    {
        TModel Get(string url, string path = "");
    }
}