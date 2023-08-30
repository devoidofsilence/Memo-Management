using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class UserRoleMapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleMapperId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("UserRole")]
        public int UserRoleId { get; set; }
        public User User { get; set; }
        public UserRole UserRole { get; set; }
    }
}
