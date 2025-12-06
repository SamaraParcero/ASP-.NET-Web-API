using APICatalog.DTOs;
using APICatalog.Models;
using APICatalog.Pagination;
using APICatalog.Repositorys;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly IRepository<Product> _repository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        [HttpGet("filter/price/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsFilterByPrice([FromQuery] ProductFilterPrice productFilterPrice)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsFilterByPriceAsync(productFilterPrice);
            return GetProducts(products);
        }

        private ActionResult<IEnumerable<ProductDTO>> GetProducts(IPagedList<Product> products)
        {
            var metadata = new
            {
                products.Count,
                products.PageSize,
                products.PageCount,
                products.TotalItemCount,
                products.HasNextPage,
                products.HasPreviousPage

            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductParameters productParameters)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync(productParameters);
            
            return GetProducts(products);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            // var products = _context.Products.AsNoTracking().Take(10).ToList();
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                if (products is null)
                {
                    return NotFound("Products not founded");
                }
                var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
                return Ok(productsDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}", Name="GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            if(id == null || id <= 0)
            {
                return BadRequest("ID invalid");
            }
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.ProductId == id);
            if (product is null)
            {
                return NotFound("Product not founded");
            }
            //Destino produto dto, recebimento de product
            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpGet("category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int id)
        {
            // var products = _context.Products.AsNoTracking().Take(10).ToList();
            var products = await _unitOfWork.ProductRepository.GetProductsByCategoryAsync(id);
            if (products is null)
            {
                return NotFound("Products not founded");
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProductDTO>> Post([FromBody]ProductDTO productDto)
        {
            if (productDto is null)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto);
            var createProduct =  _unitOfWork.ProductRepository.Create(product);
            await _unitOfWork.Commit();

            var newProductDto = _mapper.Map<ProductDTO>(createProduct);
            return new CreatedAtRouteResult("GetProduct",
                new {id = newProductDto.ProductId}, newProductDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if(patchProductDTO is null || id <= 0)
            {
                return BadRequest();
            }
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(c => c.ProductId == id);

            if(product is null)
            {
                return NotFound();
            }

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);
            patchProductDTO.ApplyTo(productUpdateRequest, ModelState);

            if(!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(productUpdateRequest, product);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Commit();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDto)
        {
            if(id != productDto.ProductId)
            {
                return BadRequest();
            }

            var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(p=> p.ProductId == id);
            if (existingProduct == null)
            {
                return BadRequest(); 
            }

            var product = _mapper.Map<Product>(productDto);
            var updatedProduct = _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Commit();

            var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

            return Ok(updatedProductDto);

               
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
        
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(p=> p.ProductId == id);

            if (product is null)
            {
                return NotFound("Product not founded");
            }

            var deletedProduct  = _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Commit();

            var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);
            return Ok(deletedProductDto);
        }
    }
}
