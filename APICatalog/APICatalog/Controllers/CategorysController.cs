using APICatalog.Context;
using APICatalog.Models;
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

        public CategorysController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Category>> GetProductsCategory()
        {
            var categorys = _context.Categorys.Include(p=> p.Products).Where(c=> c.CategoryId <=10 ).ToList();
            if (categorys is null)
            {
                return NotFound("Categories not founded");
            }
            return categorys;
        }

        [HttpGet]
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
