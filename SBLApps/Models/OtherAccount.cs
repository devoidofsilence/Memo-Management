using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class OtherAccount
    {
        [Key]
        public long DetailId { get; set; }
        [ForeignKey("BlacklistingMemoMain")]
        public long MemoId { get; set; }
        public BlacklistingMemoMain BlacklistingMemoMain { get; set; }
        public string CIF { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountScheme { get; set; }
        public string? Balance { get; set; }
        public string? AccountStatus { get; set; }
        public string? FreezeStatus { get; set; }
    }
}
