using APICatalog.Context;
using APICatalog.Filters;
using APICatalog.Models;
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
        private readonly AppDbContext _context;
        private readonly IMyService MyService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategorysController(AppDbContext context, IMyService myService, IConfiguration configuration, ILogger<CategorysController> logger)
        {
            _context = context;
            MyService = myService;
            _configuration = configuration;
            _logger = logger;
        }

        //Lê APP SETTINGS
        [HttpGet("ReadFileConfiguration")]
        public string GetValores()
        {
            var value1 = _configuration ["chave1"];
            var value2 = _configuration ["chave2"];

            var secao1 = _configuration["secao1:chave2"];
            return $"Chave1 = {value1} \nChave2 = {value2} \nSeção => Chave2 = {secao1}";
        }

        /*
        [HttpGet]
        public ActionResult<string> GetSaudationFromService([FromServices] IMyService myService, string name)
        {
            return myService.Saudacao(name);
        }
        */

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

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Category>> Get()
        {
            
            try
            {
                var categorys = _context.Categorys.ToList();
                if (categorys is null)
                {
                    return NotFound("Categories not founded");
                }
                return categorys;
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A PROBLEM HAS OCURRED WHILE DOING YOUR REQUEST");
            }

        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            //throw new Exception("Exception when get object by Id ");
            
            var category = _context.Categorys.FirstOrDefault(p => p.CategoryId == id);
            if (category is null)
            {
                return NotFound($"Category with id: {id }not founded");
            }
            return category;
        }

        [HttpPost]
        public ActionResult Post(Category category)
        {
            if (category is null)
            {
                return BadRequest();
            }

            _context.Categorys.Add(category);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCategory",
                new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var category = _context.Categorys.FirstOrDefault(p => p.CategoryId == id);
            if (category is null)
            {
                return NotFound("Category not founded");
            }
            _context.Categorys.Remove(category);
            _context.SaveChanges();

            return Ok(category);
        }
    }
}
