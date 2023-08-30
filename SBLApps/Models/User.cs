using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [NotMapped]
        [Display(Name = "User Roles")]
        public List<int> UserRoleIds { get; set; }
        [NotMapped]
        public SelectList UserRoleList { get; set; }
        [Display(Name="Is Active")]
        public bool IsActive { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
