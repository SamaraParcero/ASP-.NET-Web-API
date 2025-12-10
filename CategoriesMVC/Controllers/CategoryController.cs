using CategoriesMVC.Models;
using CategoriesMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoriesMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Index()
        {
            var result = await _categoryService.GetCategories();

            if (result == null)
            {
                return View("Error");
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult CreateNewCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCategory(CategoryViewModel categoriaVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.CreateCategory(categoriaVM);

                if (result != null)
                    return RedirectToAction(nameof(Index));
            }

            ViewBag.Erro = "Error while creating category";
            return View(categoriaVM);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var result = await _categoryService.GetCategoryById(id);
            if(result is null)
            {
                return View("Error");
            }
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryViewModel>> UpdateCategory(int id, CategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategory(id, categoryVM);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Erro = "Error while updationg category";
            return View(categoryVM);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.GetCategoryById(id);
            if (result is null)
            {
                return View("Error");
            }
            return View(result);
        }

        [HttpPost(), ActionName("DeleteCategory")]
        public async Task<ActionResult> DeletedCategory(int id)
        {
                var result = await _categoryService.DeleteCategory(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
           
            return View("Error");
        }

    }
}
