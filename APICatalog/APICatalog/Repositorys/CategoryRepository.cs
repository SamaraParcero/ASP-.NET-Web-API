using APICatalog.Context;
using APICatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Repositorys
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public Category Create(Category category)
        {
           if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

           _context.Categorys.Add(category);
            _context.SaveChanges();

            return category;
        }

        public Category Delete(int id)
        {
            var category = _context.Categorys.Find(id);
            if (category == null)
            {
                throw new ArgumentException(nameof(category));
            }
            _context.Categorys.Remove(category);
            _context.SaveChanges();

            return category;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categorys.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categorys.FirstOrDefault(c => c.CategoryId == id);
        }

        public Category Update(Category category)
        {
            if (category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return category;
        }
    }
}
