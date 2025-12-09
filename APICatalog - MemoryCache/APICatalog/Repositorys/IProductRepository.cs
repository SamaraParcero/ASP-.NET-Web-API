using APICatalog.Controllers;
using APICatalog.Models;
using APICatalog.Pagination;
using X.PagedList;

namespace APICatalog.Repositorys
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<Product> GetProducts(ProductParameters productsParameters); 
        Task<IPagedList<Product>> GetProductsAsync(ProductParameters productsParameters);
        Task<IPagedList<Product>> GetProductsFilterByPriceAsync(ProductFilterPrice productFilterPrice);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
     
    }
}
