using ClinicAPI.Models;

namespace ClinicAPI.DTOs
{
    public class UserDTOs
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? PersonalId { get; set; }
        public int Role { get; set; }
    }
}
