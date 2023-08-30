using Microsoft.AspNetCore.Mvc.Rendering;
using SBLApps.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class MemoRequestOperation
    {
        [Key]
        public long MemoRequestOperationId { get; set; }
        [ForeignKey("Request")]
        public long RequestID { get; set; }
        public BlacklistingMemoMain Request { get; set; }
        [Display(Name = "Operation")]
        [ForeignKey("Operation")]
        public int OperationID { get; set; }
        public Operation Operation { get; set; }
        [Display(Name = "Next Authority")]
        //[RequiredIf("OperationID", 11, ErrorMessage = "The Next Authority field is required.")]
        public string? OperationBy { get; set; }
        public string? RequestComingFrom { get; set; }
        [Required]
        [Display(Name = "Remarks")]
        public string OperationRemarks { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [Display(Name = "Blacklist Number")]
        public string? BlacklistNumber { get; set; }
        [Display(Name = "Blacklisting Document")]
        public string? BlacklistDocumentFullPath { get; set; }

        [NotMapped]
        [Display(Name = "Document")]
        public IFormFile DocumentHolder { get; set; }
        [NotMapped]
        public SelectList OperationList { get; set; }
        [NotMapped]
        public string OperationName { get; set; }
        [NotMapped]
        public string OperationCompletedName { get; set; }
        [NotMapped]
        public string RequestedByName { get; set; }
    }
}
