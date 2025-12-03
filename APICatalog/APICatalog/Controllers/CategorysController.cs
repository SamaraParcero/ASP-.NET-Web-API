using APICatalog.Context;
using APICatalog.DTOs;
using APICatalog.Filters;
using APICatalog.Mappings;
using APICatalog.Models;
using APICatalog.Pagination;
using APICatalog.Repositorys;
using APICatalog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalog.Controllers
{
    [EnableCors("OrigenWithAcessAllowed")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedwindow")]
    public class CategorysController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMyService MyService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategorysController(IUnitOfWork unitOfWork , IMyService myService, IConfiguration configuration, ILogger<CategorysController> logger)
        {
            _unitOfWork = unitOfWork;
            MyService = myService;
            _configuration = configuration;
            _logger = logger;
        }

        /*
        //Lê APP SETTINGS
        [HttpGet("ReadFileConfiguration")]
        public string GetValores()
        {
            var value1 = _configuration ["chave1"];
            var value2 = _configuration ["chave2"];

            var secao1 = _configuration["secao1:chave2"];
            return $"Chave1 = {value1} \nChave2 = {value2} \nSeção => Chave2 = {secao1}";
        }

        
        [HttpGet]
        public ActionResult<string> GetSaudationFromService([FromServices] IMyService myService, string name)
        {
            return myService.Saudacao(name);
        }
       

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Category>> GetProductsCategory()
        {
            _logger.LogInformation("####### GET api/categorias/produtos####");
            var categorys = _context.Categorys.Include(p=> p.Products).Where(c=> c.CategoryId <=10 ).ToList();
            if (categorys is null)
            {
                return NotFound("Categories not founded");
            }
            return categorys;
        }
         */

        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFilterByName([FromQuery] CategoryFilterName categoryFilterName)
        {
            var categoriesFilters = await _unitOfWork.CategoryRepository.GetCategoriesFilterByNameAsync(categoryFilterName);
            return GetCategories(categoriesFilters);
        }


        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoryParameters categoryParameters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(categoryParameters);
            return GetCategories(categories);
        }

        private ActionResult<IEnumerable<CategoryDTO>> GetCategories(IPagedList<Category> categories)
        {
            var metadata = new
            {
                categories.Count,
                categories.PageSize,
                categories.PageCount,
                categories.TotalItemCount,
                categories.HasNextPage,
                categories.HasPreviousPage

            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriesDto = categories.ToCategoryDTOList();
            return Ok(categoriesDto);
        }

        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        //[Authorize(Policy = "UserOnly")]
        [DisableRateLimiting]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        { 
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var categoriesDto = categories.ToCategoryDTOList();
            return Ok(categories);  
        }

        [DisableCors]
        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(c=> c.CategoryId == id);
            if (category is null)
            {
                _logger.LogWarning($"CAtegory with this id = {id} not found");
                return NotFound($"Category with id: {id }not founded");
            }

            var categoryDto = category.ToCategoryDto();
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
        {
            if (categoryDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest();
            }

            var category = categoryDto.ToCategory();

            var createCategory = _unitOfWork.CategoryRepository.Create(category);
            await _unitOfWork.Commit();

            var newCategoryDto = createCategory.ToCategoryDto();

            return new CreatedAtRouteResult("GetCategory", new { id = newCategoryDto.CategoryId }, newCategoryDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest();
            }

            var category = categoryDto.ToCategory();

             var updatedCategory = _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.Commit();

            var updatedCategoryDto = updatedCategory.ToCategoryDto();

            return Ok(updatedCategoryDto);
        }

        [HttpDelete]
        [Authorize(Policy ="AdminOnly")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(c=> c.CategoryId == id);
            if (category is null)
            {
                _logger.LogWarning($"CAtegory with this id = {id} not found");
                return NotFound("Category not founded");
            }

            var excludedCategory = _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.Commit();

            var deletedCategoryDto = excludedCategory.ToCategoryDto();

            return Ok(deletedCategoryDto);
        }
    }
}
