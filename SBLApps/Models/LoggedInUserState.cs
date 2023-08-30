namespace SBLApps.Models
{
    public class LoggedInUserState
    {
        public int LocalUserId { get; set; }
        public string Username { get; set; }
        public bool IsRequester { get; set; }
        public bool IsCurrentAuthority { get; set; }
        public bool IsFinalApprover { get; set; }
        public int RequestedStatusId { get; set; }
        public bool IsCCACUser { get; set;}
    }
}
