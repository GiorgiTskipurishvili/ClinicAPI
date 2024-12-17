using ClinicAPI.DTOs;
using ClinicAPI.Models;
using ClinicAPI.Packages;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {


        //[HttpPost]
        //public void AddUser(UserDTOs user)
        //{
        //    PKG_USERS pKG_USERS = new PKG_USERS();
        //    pKG_USERS.add_user(user);
        //}


        [HttpPost]
        public IActionResult AddUser([FromBody] UserDTOs userDTOs)
        {
            // Validate role
            if (userDTOs.Role != 2)
            {
                return BadRequest(new { message = $"Invalid role: {userDTOs.Role}. Only users with role 2 can be registered." });
            }

            try
            {
                PKG_USERS pKG_USERS = new PKG_USERS();

                // Check if email already exists
                var existingUser = pKG_USERS.get_user_by_email(userDTOs.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email already exists." });
                }

                // Add the user
                pKG_USERS.add_user(userDTOs);
                return StatusCode(201, new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                // Return internal server error
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }



        //[HttpGet]
        //public List<User> GetUsers()
        //{
        //    PKG_USERS pKG_USERS = new PKG_USERS();
        //    List<User> users = new List<User>();
        //    users = pKG_USERS.get_users();
        //    return users;
        //}

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                PKG_USERS pKG_USERS = new PKG_USERS();
                List<User> users = pKG_USERS.get_users();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //[HttpDelete("{id}")]
        //public void DeleteUser(int id)
        //{
        //    PKG_USERS pKG_USERS = new PKG_USERS();
        //    User user = new User();
        //    user.Id = id;
        //    pKG_USERS.delete_user(user);
        //}

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                PKG_USERS pKG_USERS = new PKG_USERS();
                User user = new User { Id = id };

                pKG_USERS.delete_user(user);

                return Ok($"User with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        //[HttpPut]
        //public void UpdateUser(User user)
        //{
        //    PKG_USERS pKG_USERS = new PKG_USERS();
        //    pKG_USERS.update_user(user);
        //}

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch");
            }

            try
            {
                PKG_USERS pKG_USERS = new PKG_USERS();
                pKG_USERS.update_user(user);

                return Ok($"User with ID {id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //[HttpGet("{id}")]
        //public User GetUserById(int id)
        //{
        //    PKG_USERS pKG_USERS = new PKG_USERS();
        //    User user = new User();
        //    user.Id = id;
        //    return pKG_USERS.get_user_by_id(user);
        //}

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                PKG_USERS pKG_USERS = new PKG_USERS();
                User user = new User { Id = id };
                user = pKG_USERS.get_user_by_id(user);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                PKG_USERS pkgUsers = new PKG_USERS();
                User user = pkgUsers.get_user_by_email(email);

                if (user == null)
                {
                    return NotFound($"User with email {email} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
