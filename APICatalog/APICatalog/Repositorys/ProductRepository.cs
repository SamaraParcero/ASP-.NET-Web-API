using APICatalog.Context;
using APICatalog.Models;
using APICatalog.Pagination;
using System.Linq;
using X.PagedList;
using X.PagedList.Extensions;

namespace APICatalog.Repositorys
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        {
        
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
        {
            var products = await GetAllAsync();
            var productCategory = products.Where(c => c.CategoryId == id);
            return productCategory;
        }

        /*
        public IEnumerable<Product> GetProducts(ProductParameters productsParameters)
        {
           return GetAll()
                .OrderBy(p=> p.Name)
                .Skip((productsParameters.PageNumber -1 ) * productsParameters.PageSize)
                .Take(productsParameters.PageSize).ToList();
        }
        */

        public async Task<IPagedList<Product>> GetProductsAsync(ProductParameters productParameters)
        {
            var products = await GetAllAsync();
            var orderProducts = products.OrderBy(p => p.ProductId).AsQueryable();
            var results =  orderProducts.ToPagedList(productParameters.PageNumber, productParameters.PageSize);
            return results;
        }

        public async Task<IPagedList<Product>> GetProductsFilterByPriceAsync(ProductFilterPrice productsFilterParams)
        {
            var products = await GetAllAsync();
            if(productsFilterParams.Price.HasValue && !string.IsNullOrEmpty(productsFilterParams.PriceCriterion))
            {
                if (productsFilterParams.PriceCriterion.Equals("greater", StringComparison.OrdinalIgnoreCase)) {
                    products = products.Where(p => p.Price > productsFilterParams.Price.Value).OrderBy(p => p.Price);
                } 
                else if(productsFilterParams.PriceCriterion.Equals("smaller", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price < productsFilterParams.Price.Value).OrderBy(p => p.Price);
                }
                else if (productsFilterParams.PriceCriterion.Equals("equal", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(p => p.Price == productsFilterParams.Price.Value).OrderBy(p => p.Price);
                }
            }

            var filterProducts = products.ToPagedList( productsFilterParams.PageNumber, productsFilterParams.PageSize);
            return filterProducts;
        }
    }
}
