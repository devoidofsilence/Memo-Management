using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class BlacklistingOtherPartyDetail
    {
        #region Main Entity
        [Key]
        public long OtherPartyDetailId { get; set; }
        [ForeignKey("BlacklistingMemoMain")]
        public long MemoId { get; set; }
        public int? SNo { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public decimal ShareHoldingPercentage { get; set; }
        public string? Remarks { get; set; }
        #endregion

        #region Foreign Key Relation Entities
        public BlacklistingMemoMain BlacklistingMemoMain { get; set; }
        #endregion
    }
}
