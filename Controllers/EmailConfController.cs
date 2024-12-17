using ClinicAPI.Packages;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailConfController:ControllerBase
    {
        private readonly IPKG_EMAIL_CONF _emailConf;
        private readonly ILogger<EmailConfController> _logger;

        public EmailConfController(IPKG_EMAIL_CONF emailConf, ILogger<EmailConfController> logger)
        {
            _emailConf = emailConf;
            _logger = logger;            
        }




        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerificationCode([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("SendVerificationCode called with an empty email.");
                return BadRequest(new { message = "Email is required." });
            }

            _logger.LogInformation("Sending verification code to {email}", email);

            try
            {
                var result = await _emailConf.GenerateAndStoreVerificationCodeAsync(email);
                if (result)
                {
                    _logger.LogInformation("Verification code sent successfully to {email}", email);
                    return Ok(new { message = "Verification code sent successfully." });
                }

                _logger.LogError("Failed to send verification code to {email}", email);
                return StatusCode(500, new { message = "Failed to send verification code." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending verification code to {email}", email);
                return StatusCode(500, new { message = "An error occurred while sending the verification code." });
            }
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromQuery] string email, [FromQuery] string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("VerifyCode called with an empty email or code.");
                return BadRequest(new { message = "Email and activation code are required." });
            }

            _logger.LogInformation("Verifying code for email {email}", email);

            try
            {
                var result = await _emailConf.VerifyCodeAsync(email, code);
                if (result)
                {
                    _logger.LogInformation("Activation code for {email} is valid.", email);
                    return Ok(new { message = "Activation code is valid." });
                }

                _logger.LogWarning("Activation code for {email} is invalid or expired.", email);
                return BadRequest(new { message = "Activation code is invalid or expired." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while verifying code for {email}", email);
                return StatusCode(500, new { message = "An error occurred while verifying the activation code." });
            }
        }
    }

}
