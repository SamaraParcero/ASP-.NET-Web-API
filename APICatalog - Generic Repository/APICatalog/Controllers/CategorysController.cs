using APICatalog.Context;
using APICatalog.Filters;
using APICatalog.Models;
using APICatalog.Repositorys;
using APICatalog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorysController : ControllerBase
    {
        private readonly IRepository<Category> _repository;
        private readonly IMyService MyService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategorysController(IRepository<Category> repository , IMyService myService, IConfiguration configuration, ILogger<CategorysController> logger)
        {
            _repository = repository;
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


        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        { 
            var categories = _repository.GetAll();
            return Ok(categories);  
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            
            var category = _repository.GetById(c=> c.CategoryId == id);
            if (category is null)
            {
                _logger.LogWarning($"CAtegory with this id = {id} not found");
                return NotFound($"Category with id: {id }not founded");
            }
            return Ok(category);
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest();
            }

            var createCategory = _repository.Create(category);

            return Ok(createCategory);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest();
            }

            _repository.Update(category);

            return Ok(category);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var category = _repository.GetById(c=> c.CategoryId == id);
            if (category is null)
            {
                _logger.LogWarning($"CAtegory with this id = {id} not found");
                return NotFound("Category not founded");
            }
            
            var excludedCategory = _repository.Delete(category);

            return Ok(excludedCategory);
        }
    }
}
