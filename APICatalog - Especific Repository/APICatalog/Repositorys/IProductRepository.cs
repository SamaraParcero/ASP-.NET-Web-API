using APICatalog.Models;

namespace APICatalog.Repositorys
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Product GetProductById(int id);
        Product Create(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}
