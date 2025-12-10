using CategoriesMVC.Models;

namespace CategoriesMVC.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetCategories();

        Task<CategoryViewModel> GetCategoryById(int id);

        Task<CategoryViewModel> CreateCategory(CategoryViewModel categoryVM);

        Task<bool> UpdateCategory(int id, CategoryViewModel categoryVM);

        Task<bool> DeleteCategory(int id);
    }
}
