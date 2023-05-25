using DemoToken.CustomResult;
using DemoToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ProductController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = $"{UserRole.Admin},{UserRole.User}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var resources = await _dbContext.Products.ToListAsync();
                if (resources != null && resources.Any())
                {
                    var response = new CustomStatusCode<IEnumerable<Product>>
                        (StatusCodes.Status200OK, "Get list products successfully",
                        resources, null);
                    return Ok(response);
                }
                else
                {
                    var response = new CustomStatusCode<IEnumerable<UserModel>>
                        (StatusCodes.Status404NotFound, "Not found result", null, null);
                    return NotFound(resources);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CustomStatusCode<UserModel>()
                    {
                        StatusMessage = "An error occured while retrived model",
                        Error = ex.Message
                    }); ;
            }
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomStatusCode<Product>>> GetProduct(int id)
        {
            try
            {
                var resource = await _dbContext.Products.FindAsync(id);
                if (resource == null)
                {
                    var response = new CustomStatusCode<Product>(404,
                        "Resource not found", null, null);
                    return NotFound(response);
                }
                else
                {
                    var response = new CustomStatusCode<Product>(200,
                        "Get product successfully", resource, null);

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomStatusCode<UserModel>()
                {
                    StatusMessage = "An error occurred while retrieving the model.",
                    Error = ex.Message
                });
            }
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                var resource = await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                if (resource != null)
                {
                    var response = new CustomStatusCode<Product>(201, "Resource created",
                        product, null);
                    return Ok(response);
                }
                else
                {
                    var response = new CustomStatusCode<UserModel>(400,
                      "Unable to create resource", null, null);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomStatusCode<UserModel>()
                {
                    StatusMessage = "An error occurred while retrieving the model.",
                    Error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomStatusCode<Product>>> PutProduct(Product product)
        {
            try
            {
                var existingProduct = await GetProduct(product.Id);

                if (existingProduct == null)
                {
                    _dbContext.Entry(product).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    var response = new CustomStatusCode<Product>(200,
                           "update employee successfully", product, null);
                    return Ok(response);
                }
                else
                {
                    var response = new CustomStatusCode<Product>(404,
                             "Not found product to update", null, null);
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomStatusCode<Product>()
                {
                    StatusMessage = "An error occurred while retrieving the model.",
                    Error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomStatusCode<string>>> Delete(int id)
        {
            bool resourceDeleted = false;
            var product = await _dbContext.Products.FindAsync(id);

            if (product != null)
            {
                resourceDeleted = _dbContext.Products.Remove(product) != null;
                await _dbContext.SaveChangesAsync();
            }
            if (resourceDeleted)
            {
                var response = new CustomStatusCode<string>(200,
                    "Resource deleted successfully", null, null);
                return Ok(response);
            }
            else
            {
                var response = new CustomStatusCode<string>(200,
                    "Resource not found or unable to delete", null, null);
                return NotFound(response);
            }
        }
    }
}