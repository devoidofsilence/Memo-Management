using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class LinkedEntitiesDetail
    {
        [Key]
        public long DetailId { get; set; }
        [ForeignKey("BlacklistingMemoMain")]
        public long MemoId { get; set; }
        public BlacklistingMemoMain BlacklistingMemoMain { get; set; }
        public string MainAccountNumber { get; set; }
        public string Cif { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Balance { get; set; }
        public string? AccountStatus { get; set; }
        public string? FreezeStatus { get; set; }
    }
}
