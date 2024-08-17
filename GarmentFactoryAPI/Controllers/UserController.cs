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
using Microsoft.AspNetCore.Authorization;
using GarmentFactoryAPI.Services;
using GermentFactory.Services; // Ensure this namespace is correct and the project reference is added

namespace DetMayDemoApp.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService; // Add this line


        public UserController(DataContext context, IConfiguration configuration, UserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        [Route("api/User/Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            var registeredUser = await _userService.RegisterAsync(userDto);
            if (registeredUser == null)
            {
                return Conflict("Username already exists");
            }

            return CreatedAtAction("GetUser", new { id = registeredUser.Id }, registeredUser);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        [Route("api/User/GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAll();
            return Ok(new { UserDTO = users });
        }


        [HttpGet]
        [Route("api/User/GetUser")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetById1(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }


        [HttpPost]
        [Route("api/User/Login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginDTO user)
        {
            var userInDb = await _userService.AuthenticateAsync(user.Username,user.Password);
            if (userInDb != null)
            {
                var claims = new List<Claim>
{
    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? string.Empty),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim("Username", userInDb.Username),
    new Claim("Password", userInDb.Password),
    new Claim("roleId", userInDb.RoleId.ToString()) // Đảm bảo roleId được thêm vào
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

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete]
        [Route("api/User/DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteById1(id);
            if (result.Code == Const.SUCCESS_DELETE_CODE)
            {
                return Ok(result.Message);
            }

            return NotFound(result.Message);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        [Route("api/User/UpdateUser")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] RegisterDTO user)
        {
            // Retrieve the user by ID
            var userResult = await _userService.GetById1(id);

            if (userResult == null || userResult.Data == null)
            {
                return NotFound("User not found");
            }

            var userUpdate = userResult.Data as User;
            if (userUpdate == null)
            {
                return StatusCode(500, "Error retrieving user data");
            }

            // Update the user's properties
            userUpdate.Username = user.Username;
            userUpdate.Password = user.Password;
            userUpdate.RoleId = user.roleId; // Assuming roles are updated as well

            try
            {
                // Attempt to update the user in the database
                await _userService.Save(userUpdate);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the update process
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
