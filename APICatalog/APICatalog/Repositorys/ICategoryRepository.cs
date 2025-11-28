using APICatalog.Models;

namespace APICatalog.Repositorys
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        Category Create(Category category);
        Category Update(Category category);
        Category Delete(int id);

    }
}
