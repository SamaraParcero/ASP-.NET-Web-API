using APICatalog.Context;
using APICatalog.Models;
using APICatalog.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace APICatalog.Repositorys
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(AppDbContext context) : base(context) 
        {
    
        }

        public async Task<IPagedList<Category>>  GetCategoriesAsync(CategoryParameters categoryParameters)
        {
            var categories = await GetAllAsync();
            var orderCategories = categories.OrderBy(p=>p.CategoryId).AsQueryable();
            //var result = PagedList<Category>.ToPagedList(orderCategories, categoryParameters.PageNumber, categoryParameters.PageSize);
            var result =  orderCategories.AsQueryable().ToPagedList(categoryParameters.PageNumber, categoryParameters.PageSize);
            return result;
        }

        public async Task<IPagedList<Category>> GetCategoriesFilterByNameAsync(CategoryFilterName categoryParams)
        {
            var categories = await GetAllAsync();
            if (!string.IsNullOrEmpty(categoryParams.Name))
            {
                categories = categories.Where(c=>c.Name.Contains(categoryParams.Name, StringComparison.OrdinalIgnoreCase));
            }

            //var filterCategories = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoryParams.PageNumber, categoryParams.PageSize);
            var filterCategories =  categories.AsQueryable().ToPagedList(categoryParams.PageNumber, categoryParams.PageSize);
            return filterCategories;
        }
    }
}
