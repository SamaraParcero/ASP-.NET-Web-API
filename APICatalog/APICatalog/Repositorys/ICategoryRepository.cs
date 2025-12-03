using APICatalog.Models;
using APICatalog.Pagination;
using X.PagedList;

namespace APICatalog.Repositorys
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IPagedList<Category>> GetCategoriesFilterByNameAsync(CategoryFilterName categoryFilterName);
        Task<IPagedList<Category>> GetCategoriesAsync(CategoryParameters categoryParameters);
    }
}
