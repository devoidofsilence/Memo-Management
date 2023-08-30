using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class BlacklistingDocumentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DocumentDetailId { get; set; }
        [ForeignKey("BlacklistingMemoMain")]
        public long MemoId { get; set; }
        public BlacklistingMemoMain BlacklistingMemoMain { get; set; }
        // Other properties...
        public int? DocumentTypeId { get; set; }
        public string? DocumentFullPath { get; set; }
        [NotMapped]
        public IFormFile DocumentHolder { get; set; }
    }
}
