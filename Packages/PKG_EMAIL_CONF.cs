using ClinicAPI.Package;
using ClinicAPI.Services;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicAPI.Packages
{
    public interface IPKG_EMAIL_CONF
    {
        Task<bool> GenerateAndStoreVerificationCodeAsync(string email);
        Task<bool> VerifyCodeAsync(string email, string code);
    }

    public class PKG_EMAIL_CONF : PKG_BASE, IPKG_EMAIL_CONF
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<PKG_EMAIL_CONF> _logger;

        public PKG_EMAIL_CONF(IEmailService emailService, ILogger<PKG_EMAIL_CONF> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<bool> GenerateAndStoreVerificationCodeAsync(string email)
        {
            // Generate a verification code
            var verificationCode = new Random().Next(100000, 999999).ToString();

            try
            {
                using (var connection = new OracleConnection(ConnStr))
                {
                    await connection.OpenAsync();

                    using (var command = new OracleCommand("PKG_GIORGITSK_EMAIL_CONF.create_mail_verification_code", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_mail", OracleDbType.Varchar2).Value = email;
                        command.Parameters.Add("p_code", OracleDbType.Varchar2).Value = verificationCode;

                        await command.ExecuteNonQueryAsync();
                    }
                }

                // Send the verification email
                await _emailService.SendVerificationEmailAsync(email, verificationCode);

                _logger.LogInformation($"Verification code sent to {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to generate or send verification code for {email}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            try
            {
                using (var connection = new OracleConnection(ConnStr))
                {
                    await connection.OpenAsync();

                    using (var command = new OracleCommand("PKG_GIORGITSK_EMAIL_CONF.verify_code", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                        command.Parameters.Add("p_code", OracleDbType.Varchar2).Value = code;
                        //var isValidParam = new OracleParameter("p_isValid", OracleDbType.Int32)
                        //{
                        //    Direction = ParameterDirection.Output
                        //};
                        //command.Parameters.Add(isValidParam);
                        command.Parameters.Add("p_is_valid", OracleDbType.Int32).Direction = ParameterDirection.Output;

                        await command.ExecuteNonQueryAsync();

                        //var isValid = Convert.ToInt32(isValidParam.Value);
                        //return isValid == 1;
                        return Convert.ToInt32(command.Parameters["p_is_valid"].Value.ToString()) == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to verify code for {email}: {ex.Message}");
                return false;
            }
        }


    }
}
