using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class Operation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OperationID { get; set; }
        public string OperationName { get; set; }
        public string OperationCompletedName { get; set; }
    }
}
