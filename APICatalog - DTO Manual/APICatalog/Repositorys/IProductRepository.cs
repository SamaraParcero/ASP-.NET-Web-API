using APICatalog.Models;

namespace APICatalog.Repositorys
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsByCategory(int id);
    }
}
