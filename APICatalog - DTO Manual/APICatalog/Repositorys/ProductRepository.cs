using APICatalog.Context;
using APICatalog.Models;

namespace APICatalog.Repositorys
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext context) : base(context)
        {
        
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            return GetAll().Where(c => c.CategoryId == id);
        }
    }
}
