using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DetMayDemoApp.Models;
using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Models;
using Microsoft.AspNetCore.Authorization; // Ensure this namespace is correct and the project reference is added

namespace DetMayDemoApp.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public UserController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("api/User/Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            // Check if the username already exists
            bool usernameExists = await _context.Users.AnyAsync(u => u.Username == userDto.Username);
            if (usernameExists)
            {
                return Conflict("Username already exists");
            }

            // Map the RegisterDTO to User entity
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                RoleId = userDto.roleId,

                // Map other properties as needed
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { username = user.Username }, user);
        }


        [HttpGet]
        [Route("api/User/GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet]
        [Route("api/User/GetUser")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser(int id)
        {
            return await _context.Users.ToListAsync();
        }


        [HttpPost]
        [Route("api/User/Login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginDTO user)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);
            if (userInDb != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Username", userInDb.Username),
                    new Claim("Password", userInDb.Password)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signIn
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = tokenString, User = user });
            }
            return BadRequest("Invalid credentials");
        }
    }
}
