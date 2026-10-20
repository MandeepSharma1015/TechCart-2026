using Core.Entity;
using Core.UseCases;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository _repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var product = await _repo.GetProductsAsync();
            if(product == null || !product.Any())
            {
                return NotFound("No Product Found");
            }
            return Ok(product);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if(product == null)
            {
                return NotFound("No Product Found");
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (product == null)
            {
                return NotFound("Product can not be null");
            }
            _repo.AddProduct(product);
            if(!await _repo.SaveChangesAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving product to the database");
            }
            
            return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id,Product product)
        {
            if(id != product.Id) 
            {
                return BadRequest("Product Id MisMatch");
            }
            _repo.UpdateProduct(product);
            try
            {
                await _repo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repo.ProductExists(id))
                {
                    return NotFound("Product not found");
                }
                throw;
            }
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if(product == null)
            {
                return NotFound("Product not found");
            }
            _repo.DeleteProduct(product);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
    }
}
