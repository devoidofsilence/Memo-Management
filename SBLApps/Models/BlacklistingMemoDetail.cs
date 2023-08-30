using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class BlacklistingMemoDetail
    {
        [Key]
        public long MemoDetailId { get; set; }
        [ForeignKey("BlacklistingMemoMain")]
        public long MemoId { get; set; }
        public BlacklistingMemoMain BlacklistingMemoMain { get; set; }
        public int? SNo { get; set; }
        public string? AccountNumber { get; set; }
        public string? ChequeIssueDate { get; set; }
        public string? ChequeReturnDate { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal ChequeAmount { get; set; }
        public string? ReasonOfReturn { get; set; }
        //public string? CompanyFullName { get; set; }
        //public string? Address { get; set; }
        //public decimal ShareHoldingPercentage { get; set; }
        //public string? Remarks { get; set; }
    }
}
