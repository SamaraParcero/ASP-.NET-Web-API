using CategoriesMVC.Models;
using CategoriesMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CategoriesMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private string token = string.Empty;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
        {
            var result = await _productService.GetProducts(GetTokenJwt());

            if(result is null)
            {
                return View("Error");
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewProduct()
        {
            ViewBag.CategoriaId =
               new SelectList(await _categoryService.GetCategories(), "CategoriaId", "Nome");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> CreateNewProduct(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProduct(productVM, GetTokenJwt());

                if (result != null)
                    return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.CategoriaId =
                new SelectList(await _categoryService.GetCategories(), "CategoriaId", "Nome");
            }
            return View(productVM);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsProduct(int id)
        {
            var product = await _productService.GetProductById(id, GetTokenJwt());

            if (product is null)
                return View("Error");

            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var result = await _productService.GetProductById(id, GetTokenJwt());

            if (result is null)
                return View("Error");

            ViewBag.CategoriaId =
              new SelectList(await _categoryService.GetCategories(), "CategoriaId", "Nome");

            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> UpdateProduct(int id, ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProduct(id, productVM, GetTokenJwt());

                if (result)
                    return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.GetProductById(id, GetTokenJwt());

            if (result is null)
                return View("Error");

            return View(result);
        }

        [HttpPost(), ActionName("DeleteProduct")]
        public async Task<IActionResult> DeletedProduct(int id)
        {
            var result = await _productService.DeleteProduct(id, GetTokenJwt());

            if (result)
                return RedirectToAction("Index");

            return View("Error");
        }


        private string GetTokenJwt()
        {
            if (HttpContext.Request.Cookies.ContainsKey("X-Acess-Token"))
                token = HttpContext.Request.Cookies["X-Acess-Token"].ToString();

            return token;
        }
    }
}
