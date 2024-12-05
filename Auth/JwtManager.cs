//using ClinicAPI.Models;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace ClinicAPI.Auth
//{
//    public interface IJwtManager
//    {

//    }
//    public class JwtManager: IJwtManager
//    {
//        private readonly IConfiguration _configuration;


//        public JwtManager(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }


//        public Token GetToken(User user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);


//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new Claim[]
//    {
//                    new Claim("email",user.Email, ClaimTypes.Name),
//                    new Claim("password",user.Password, ClaimTypes.NameIdentifier),
//                    new Claim("role",user.Role.ToString(),ClaimTypes.Role)
//    }),
//                Expires = DateTime.UtcNow.AddMinutes(10),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
//            };

//            var tokenData = tokenHandler.CreateToken(tokenDescriptor);
//            var token = new Token { AccessToken = tokenHandler.WriteToken(tokenData) };
//            return token;

//        }
//    }

//    public class Token
//    {
//        public string? AccessToken { get; set; }
//    }
//}



//using ClinicAPI.Models;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace ClinicAPI.Auth
//{
//    public interface IJwtManager
//    {
//        Token GetToken(User user);
//    }

//    public class JwtManager : IJwtManager
//    {
//        private readonly IConfiguration _configuration;

//        public JwtManager(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public Token GetToken(User user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                    new Claim(ClaimTypes.Name, user.Email),
//                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                    new Claim(ClaimTypes.Role, user.Role.ToString())
//                }),
//                Expires = DateTime.UtcNow.AddMinutes(10),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
//            };

//            var tokenData = tokenHandler.CreateToken(tokenDescriptor);
//            return new Token { AccessToken = tokenHandler.WriteToken(tokenData) };
//        }
//    }

//    public class Token
//    {
//        public string? AccessToken { get; set; }
//    }
//}



using ClinicAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicAPI.Auth
{
    public interface IJwtManager
    {
        Token GetToken(User user);
        Token GetToken(Doctor doctor);
    }

    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;

        public JwtManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token GetToken(User user)
        {
            return GenerateToken(user.Email, user.Id.ToString(), "User");
        }

        public Token GetToken(Doctor doctor)
        {
            return GenerateToken(doctor.Email, doctor.Id.ToString(), "Doctor");
        }

        private Token GenerateToken(string email, string id, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };

            var tokenData = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { AccessToken = tokenHandler.WriteToken(tokenData) };
        }
    }

    public class Token
    {
        public string? AccessToken { get; set; }
    }
}
