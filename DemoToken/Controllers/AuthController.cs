using DemoToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext dbcontext;
        private readonly IConfiguration configuration;

        public AuthController(DatabaseContext _dbcontext, IConfiguration _configuration)
        {
            dbcontext = _dbcontext;
            configuration = _configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserLogin userLogin)
        {
            var user = Authenicate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                var userToken = new UserModel
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = user.Role
                };
                return Ok(new { token, userToken });
            }
            return NotFound("user not found");
        }

        // to authentication user
        private UserModel Authenicate(UserLogin userLogin)
        {
            var listUser = dbcontext.Users.ToList();
            if (listUser != null && listUser.Count > 0)
            {
                var currentUser = listUser.FirstOrDefault(u => u.Email.ToLower() == userLogin.Email && u.Password == userLogin.Password);
                return currentUser;
            }
            return null;
        }

        // to generate token
        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Username",user.UserName),
                new Claim("Email",user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}