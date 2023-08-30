using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        public string? RedirectUrl { get; set; }
    }

    public class EmployeeModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
    }
}
