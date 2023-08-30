namespace SBLApps.Models
{
    public class EmailDetail
    {
        public long MemoId { get; set; }
        public int? OperationType { get; set; }
        public string AssigneeEmail { get; set; }
        public string AssigneeFullName { get; set; }
        public string MemoReferenceNumber { get; set; }
        public string AssignerFullName { get; set; }
        public string EmailSender { get; set;}
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
