using crud.Data;
using crud.Interface;
using crud.Model.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly IUserService _userService;
        public CrudController(IUserService userService)
        {
            _userService = userService; 
        }

        [HttpPost("addUser")]
        public async  Task<IActionResult> SaveUserInfo([FromBody] userdto request)
        {
            if (!ModelState.IsValid)
            {
               
                return BadRequest(ModelState);
            }
            var result = await _userService.SaveUser(request);

            if (result)
            {
                return Ok("User saved successfully");
            }
            else
            {
                return StatusCode(500, "An error occurred while saving the user");
            }

        }
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();

                if (users.Any())
                {
                    return Ok(users);  // Return the list of users
                }
                else
                {
                    return NotFound("No users found");  // If no users are found
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the users");
            }
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] userdto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUser(id, request);
            if (result)
            {
                return Ok("User updated successfully");
            }
            else
            {
                return NotFound($"User with id {id} not found or an error occurred");
            }
        }

        // DELETE: api/User/deleteUser/5
        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return NotFound($"User with id {id} not found or an error occurred");
            }
        }

        [HttpPost("getJwtToken/{userid}")]
        public async Task<IActionResult> GetJWTToken(int userid)
        {
            try
            {
                var token = await _userService.GenerateToken(userid);

                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(new { token }); // ✅ Return as JSON object
                }
                else
                {
                    return NotFound(new { message = "Unable to generate the JWT token" });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while generating the JWT Token" });
            }
        }

    }
}
