using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBLApps.Models
{
    public class BlacklistingMemoMain
    {
        #region Main Entity
        [Display(Name = "Memo Id")]
        [Key]
        public long MemoId { get; set; }
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }
        [Display(Name = "Final Approver")]
        public string FinalApproverSAMName { get; set; }
        public int MemoTypeId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        public string CIF { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Account Holder's Name")]
        public string AccountHolderName { get; set; }
        [Display(Name = "Type of Customer")]
        public int CustomerTypeId { get; set; }
        [Display(Name = "Is the customer Loan Customer of the bank?")]
        public bool IsLoanCustomer { get; set; }
        [Display(Name = "Total Loan Outstanding")]
        public decimal? TotalLoanOutstanding { get; set; }
        [Display(Name = "Name of RO/RM")]
        public string? NameOfRORM { get; set; }
        [Display(Name = "Name of Payee")]
        public string NameOfPayee { get; set; }
        public string Initiator { get; set; }
        [Display(Name = "Blacklisting Requested By")]
        public string BlacklistingRequestedBy { get; set; }
        [Required]
        [Display(Name = "Blacklisting Application Received Date")]
        public DateTime? BlacklistingApplicationReceivedDate { get; set; } = null;
        [Display(Name = "Address of Requesting Person")]
        public string AddressOfRequestingPerson { get; set; }
        [Display(Name = "ID Details")]
        public string? StaffIDNoOfRequestingPerson { get; set; }
        [Display(Name = "Contact Number of Requesting Person")]
        public string ContactNumberOfRequestingPerson { get; set; }
        [Display(Name = "Total Cheque Amount")]
        public decimal TotalChequeAmount { get; set; }
        [Display(Name = "Request Status")]
        public int RequestStatusId { get; set; }
        [DisplayName("Requesting For")]
        [ForeignKey("Operation")]
        public int LatestOperationId { get; set; }
        [Display(Name = "Next Authority")]
        public string? NextAuthority { get; set; }
        [Display(Name = "Requirement Remarks")]
        public string MemoRequirementRemarks { get; set; }
        #endregion

        #region Detail Tables
        public List<BlacklistingMemoDetail>? BlacklistingMemoDetails { get; set; }
        public List<BlacklistingOtherPartyDetail>? BlacklistingOtherPartyDetails { get; set; }
        public List<BlacklistingDocumentDetail>? BlacklistingDocumentDetails { get; set; }
        public List<OtherAccount>? OtherAccounts { get; set; }
        public List<LinkedEntitiesDetail>? LinkedEntitiesDetails { get; set; }
        #endregion

        #region Foreign Key Relation Entities
        public MemoType MemoType { get; set; }
        public Branch Branch { get; set; }
        public Department Department { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public Operation Operation { get; set; }
        public CustomerType CustomerType { get; set; }
        #endregion

        #region Not Mapped(For Operations)
        [NotMapped]
        //Dynamic Grid Data
        public string StringifiedBlacklistingMemoDetails { get; set; }

        [NotMapped]
        //Dynamic Grid Data
        public string StringifiedBlacklistingOtherPartyDetails { get; set; }
        [NotMapped]
        //Dynamic Grid Data
        public string StringifiedBlacklistingDocumentDetails { get; set; }
        [NotMapped]
        public string FinalApproverFullName { get; set; }
        [NotMapped]
        public string StringifiedOtherAccounts { get; set; }
        [NotMapped]
        public string StringifiedLinkedEntitiesDetails { get; set; }
        [NotMapped]
        public SelectList BranchList { get; set; }
        [NotMapped]
        public SelectList DepartmentList { get; set; }
        [NotMapped]
        public SelectList RequestStatusList { get; set; }
        [NotMapped]
        public SelectList MemoTypeList { get; set; }
        [NotMapped]
        public SelectList CustomerTypeList { get; set; }
        [NotMapped]
        public SelectList DocumentTypeList { get; set; }
        [NotMapped]
        public SelectList OperationList { get; set; }
        [NotMapped]
        public AuthorityManager AuthorityManager { get; set; }
        [NotMapped]
        public List<MemoRequestOperation> MemoRequestOperations { get; set; }
        [NotMapped]
        [DisplayName("Last Requested Status")]
        public string RequestStatusName { get; set; }
        [NotMapped]
        [DisplayName("Customer Type")]
        public string CustomerTypeName { get; set; }

        [NotMapped]
        [DisplayName("Forward To  User")]
        public string? ForwardToUser { get; set; }

        [NotMapped]
        [DisplayName("Forward Remarks")]
        public string? ForwardRemarks { get; set; }

        #endregion
    }
}