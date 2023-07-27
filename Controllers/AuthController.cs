using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

//User -> can only view products

//Editor User -> can only view products and edit,add,delete product

//Admin -> can also view transactions beside the products

//Editor Admin -> can view everything and edit evrything

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public readonly List<UserModel> _users = new()
        {
            new UserModel()
            {
                ID = 1,
                Username = "Ali",
                Password = "Test",
                Roles = new List<string>()
                {
                    "User"
                }
            },

            new UserModel()
            {
                ID=2,
                Username="Nazim",
                Password="Test1",
                Roles = new List<string>()
                {
                   "Admin"
                }
            },

            new UserModel()
            {
                ID=3,
                Username="Rubabe",
                Password="Test2",
                Roles = new List<string>()
                {
                    "Admin","Editor User"
                }
            },

            new UserModel()
            {
                ID=4,
                Username="Niyameddin",
                Password="Test3",
                Roles= new List<string>()
                {
                    "Editor Admin"
                }
            }
        };

        public readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("generateToken")]
        public ActionResult<object> GenerateToken(LoginModel login)
        {
            var loggedUser = _users.FirstOrDefault(u => u.Username == login.Usernamae && u.Password == login.Password);

            if(loggedUser is null)
            {
               return NotFound();
            }

            string loggedUserToken = GenerateToken(loggedUser);

            return new
            {

                Id = loggedUser.ID,

                token = loggedUserToken

            };

        }

        private string GenerateToken(UserModel user)
        {

            //Step 1: Create token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //Step 2: Generate key for JWT
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);

            //Step 3: Create Claims for JWT
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),

                new Claim(ClaimTypes.Name, user.Username!.ToString())

            };

            if (user.Roles.Count > 0)
            {
                // Adding Roles to the Claim if role exists
                claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            }

            // Token Parameters
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                //Create idendity for ClaimsPrincipal
                Subject = new ClaimsIdentity(claims),
                //Set Expire data for token
                Expires = DateTime.UtcNow.AddHours(1),
                // Set Algorithm for security key of the token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Generate token
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //Return token
            return tokenHandler.WriteToken(token);

        }

    }
}