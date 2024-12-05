////using ClinicAPI.Auth;
////using Microsoft.AspNetCore.Mvc;

////namespace ClinicAPI.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class LoginController : Controller
////    {

////        private readonly IJwtManager jwtManager;
////        public LoginController(IJwtManager jwtManager)
////        {
////            this.jwtManager = jwtManager;
////        }






////    }
////}

////using ClinicAPI.Auth;
////using ClinicAPI.Models;
////using ClinicAPI.Packages;
////using Microsoft.AspNetCore.Mvc;

////namespace ClinicAPI.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class LoginController : ControllerBase
////    {
////        private readonly IJwtManager _jwtManager;

////        public LoginController(IJwtManager jwtManager)
////        {
////            _jwtManager = jwtManager;
////        }

////        [HttpPost]
////        public IActionResult Login([FromBody] Login login)
////        {
////            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
////            {
////                return BadRequest("Invalid login request.");
////            }

////            // Check if user exists with provided credentials
////            PKG_USERS pkgUsers = new PKG_USERS();
////            var user = pkgUsers.get_user_by_email(login.Email);

////            if (user == null || user.Password != login.Password)
////            {
////                return Unauthorized("Invalid email or password.");
////            }

////            // Generate JWT token
////            var token = _jwtManager.GetToken(user);
////            return Ok(token);
////        }
////    }
////}


//using ClinicAPI.Auth;
//using ClinicAPI.Models;
//using ClinicAPI.Packages;
//using Microsoft.AspNetCore.Mvc;

//namespace ClinicAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : ControllerBase
//    {
//        private readonly IJwtManager _jwtManager;

//        public LoginController(IJwtManager jwtManager)
//        {
//            _jwtManager = jwtManager;
//        }

//        [HttpPost]
//        public IActionResult Login([FromBody] Login login)
//        {
//            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
//            {
//                return BadRequest("Invalid login request.");
//            }

//            // Check if the credentials belong to a User
//            PKG_USERS pkgUsers = new PKG_USERS();
//            var user = pkgUsers.get_user_by_email(login.Email);

//            if (user != null && user.Password == login.Password)
//            {
//                // Generate JWT token with User role
//                var token = _jwtManager.GetToken(user);
//                return Ok(token);
//            }

//            // If not a User, check if the credentials belong to a Doctor
//            PKG_DOCTORS pkgDoctors = new PKG_DOCTORS();
//            var doctor = pkgDoctors.get_doctor_by_email(login.Email);

//            if (doctor != null && doctor.Password == login.Password)
//            {
//                // Generate JWT token with Doctor role
//                var token = _jwtManager.GetToken(doctor);
//                return Ok(token);
//            }

//            // If no matching user or doctor is found, return Unauthorized
//            return Unauthorized("Invalid email or password.");
//        }
//    }
//}


//using ClinicAPI.Auth;
//using ClinicAPI.Models;
//using ClinicAPI.Packages;
//using Microsoft.AspNetCore.Mvc;

//namespace ClinicAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : ControllerBase
//    {
//        private readonly IJwtManager _jwtManager;

//        public LoginController(IJwtManager jwtManager)
//        {
//            _jwtManager = jwtManager;
//        }

//        [HttpPost]
//        public IActionResult Login([FromForm] Login login)
//        {
//            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
//            {
//                return BadRequest("Invalid login request.");
//            }

//            // Check if credentials belong to a User or Admin
//            PKG_USERS pkgUsers = new PKG_USERS();
//            var user = pkgUsers.get_user_by_email(login.Email);

//            if (user != null && user.Password == login.Password)
//            {
//                // Generate JWT token
//                var token = _jwtManager.GetToken(user);

//                // Determine role based on user data
//                var role = user.Role == 1 ? "Admin" : "User";

//                return Ok(new
//                {
//                    id = user.Id,
//                    token = token.AccessToken,
//                    role = role,
//                    firstName = user.FirstName,
//                    lastName = user.LastName
//                });
//            }

//            // Check if credentials belong to a Doctor
//            PKG_DOCTORS pkgDoctors = new PKG_DOCTORS();
//            var doctor = pkgDoctors.get_doctor_by_email(login.Email);

//            if (doctor != null && doctor.Password == login.Password)
//            {
//                // Generate JWT token for Doctor
//                var token = _jwtManager.GetToken(doctor);
//                return Ok(new
//                {
//                    id = doctor.Id,
//                    token = token.AccessToken,
//                    role = "Doctor",
//                    firstName = doctor.FirstName,
//                    lastName = doctor.LastName
//                });
//            }

//            // No matching user or doctor found
//            return Unauthorized("Invalid email or password.");
//        }
//    }
//}


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



