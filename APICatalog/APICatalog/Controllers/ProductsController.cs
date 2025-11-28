using APICatalog.Context;
using APICatalog.Models;
using APICatalog.Repositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository product)
        {
            _productRepository = product;
        }

        /*
       //COM ASYNC
       [HttpGet]
       public async Task<ActionResult<IEnumerable<Product>>> GetProductAsync()
       {
           return await _context.Products.ToListAsync();
       }

       //Segundo parâmetro

      [HttpGet("{id}/{nome=Caderno}", Name="GetProduct")]
       public ActionResult<Product> Get(int id, [[BindRequired] string nome) //Com o bind required é obrigatório ser fornecido  na query string
       {
       var parametro = nome;
           var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
           if (product is null)
           {
               return NotFound("Product not founded");
           }
           return product;
       }

       //Definição de nome
       //Usar mais de um endpoint
       [HttpGet("first")]
       [HttpGet("/first")]
       public ActionResult<Product> GetFirst()
       {
           var product = _context.Products.FirstOrDefault();
           if (product is null)
           {
               return NotFound("Products not founded");
           }
           return product;
       }
       */

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            // var products = _context.Products.AsNoTracking().Take(10).ToList();
            var products = _productRepository.GetProducts().ToList();
            if (products is null)
            {
                return NotFound("Products not founded");
            }
            return Ok(products);
        }

        [HttpGet("{id:int}", Name="GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product is null)
            {
                return NotFound("Product not founded");
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post([FromBody]Product product)
        {
            if (product is null)
            {
                return BadRequest();
            }

            var createProduct = _productRepository.Create(product);

            return new CreatedAtRouteResult("GetProduct",
                new {id = product.ProductId}, createProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if(id != product.ProductId)
            {
                return BadRequest();
            }

            bool updatedProduct = _productRepository.Update(product);

            if (updatedProduct)
            {
                return Ok(product);
            }
            else
            {
                return StatusCode(500, $"Failed to Update Product");
            }

               
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
        
            var product = _productRepository.Delete(id);

            if (product)
            {
                return Ok(product);
            }
            else
            {
                return StatusCode(500, $"Failed to Delete Product");
            }

           
        }
    }
}
