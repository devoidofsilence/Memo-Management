using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class CustomerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerTypeId { get; set; }
        [DisplayName("Customer Type")]
        public string CustomerTypeName { get; set; }
    }
}
