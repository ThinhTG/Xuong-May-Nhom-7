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
using GarmentFactoryAPI.Pagination;

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
        [Route("api/User/GetUsersPaged")]
        [ProducesResponseType(200, Type = typeof(PagedResult<User>))]
        public async Task<ActionResult<PagedResult<User>>> GetUsersPaged(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var usersResult = await _userService.GetAll();
            var users = usersResult.Data as IEnumerable<User>; // Ensure Data is cast to IEnumerable<User>
            if (users == null)
            {
                return StatusCode(500, "Error retrieving user data");
            }

            var activeUsers = users;

            var totalActiveUsers = activeUsers.Count();

            var pagedUsers = activeUsers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Password = u.Password,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive,
                    // Add other properties if necessary
                })
                .ToList();

            var result = new PagedResult<User>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalActiveUsers,
                Items = pagedUsers
            };

            return Ok(result);
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
            if (result.Message == Const.FAIL_DELETE_MSG)
            {
                return NotFound();
            }
            return Ok(result.Message);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        [Route("api/User/UpdateUser")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO userDto)
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

            // Check if the new username already exists (and isn't the current user's username)
            var existingUserResult = await _userService.GetByName(userDto.Username);
            var existingUser = existingUserResult.Data as User;
            if (existingUser != null && existingUser.Id != id)
            {
                return Conflict("Username already exists");
            }

            // Update the user's properties
            userUpdate.Username = userDto.Username;
            userUpdate.Password = userDto.Password;
            userUpdate.RoleId = userDto.RoleId;
            userUpdate.IsActive = userDto.IsActive;

            try
            {
                // Attempt to update the user in the database
                await _userService.Update(userUpdate);
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
