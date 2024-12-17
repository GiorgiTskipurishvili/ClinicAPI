using ClinicAPI.Auth;
using ClinicAPI.Models;
using ClinicAPI.Packages;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtManager _jwtManager;
        private readonly PKG_DOCTORS _pkgDoctors;

        public LoginController(IJwtManager jwtManager, PKG_DOCTORS pkgDoctors)
        {
            _jwtManager = jwtManager;
            _pkgDoctors = pkgDoctors;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Invalid login request.");
            }

            // Check Users table for User or Admin
            PKG_USERS pkgUsers = new PKG_USERS();
            var existUser = pkgUsers.get_user_by_email(login.Email);

            if (existUser != null && existUser.Password == login.Password)
            {
                // Assuming role value for Admin is 1; update it if different
                var role = existUser.Role == 0 ? "Admin" : "User";
                var token = _jwtManager.GetToken(existUser);
                return Ok(new
                {
                    id = existUser.Id,
                    token = token.AccessToken,
                    role = role,
                    firstName = existUser.FirstName,
                    lastName = existUser.LastName
                });
            }

            // Check Doctors table for Doctor role
            var existDoctor = _pkgDoctors.get_doctor_by_email(login.Email);

            if (existDoctor != null && existDoctor.Password == login.Password)
            {
                string photoBase64 = existDoctor.Photo != null ? Convert.ToBase64String(existDoctor.Photo) : null;

                var token = _jwtManager.GetToken(existDoctor);
                return Ok(new
                {
                    id = existDoctor.Id,
                    token = token.AccessToken,
                    role = "Doctor",
                    firstName = existDoctor.FirstName,
                    lastName = existDoctor.LastName,
                    photo = photoBase64
                });
            }

            // No matching user or doctor found
            return Unauthorized("Invalid email or password.");
        }



    }
}



