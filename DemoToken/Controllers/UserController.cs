using DemoToken.CustomResult;
using DemoToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public UserController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        //For admin Only
        [HttpGet]
        [Route("Admins")]
        [Authorize(Roles = $"{UserRole.Admin},{UserRole.User}")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            try
            {
                var resources = await _dbContext.Users.ToListAsync();
                if (resources != null && resources.Any())
                {
                    var response = new CustomStatusCode<IEnumerable<UserModel>>
                        (StatusCodes.Status200OK, "Get list successfully", resources, null);
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

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomStatusCode<UserModel>>> GetUser(int id)
        {
            try
            {
                var resource = await _dbContext.Users.FindAsync(id);
                if (resource == null)
                {
                    var response = new CustomStatusCode<UserModel>(404,
                        "Resource not found or unable to delete", null, null);
                    return NotFound(response);
                }
                else
                {
                    var response = new CustomStatusCode<UserModel>(200,
                        "Get employee successfully", resource, null);

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> addUser(UserModel userModel)
        {
            try
            {
                var resource = await _dbContext.Users.AddAsync(userModel);
                await _dbContext.SaveChangesAsync();
                if (resource != null)
                {
                    var response = new CustomStatusCode<UserModel>(201, "Resource created",
                       userModel, null);
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
    }
}