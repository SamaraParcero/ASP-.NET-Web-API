using APICatalog.Context;
using APICatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Repositorys
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext context) : base(context) 
        {
    
        }

    }
}
