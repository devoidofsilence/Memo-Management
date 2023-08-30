namespace SBLApps.Enums
{
    public enum RequestStatusEnum
    {
        Initiated = 1,
        Recommended = 2,
        Approved = 3,
        Returned = 4,
        ForRefer = 5,
        Referred = 6,
        ForReview = 7,
        Reviewed = 8,
        Rejected = 9,
        Noted = 10,
        Completed = 11
    }

    public enum OperationEnum
    {
        Initiate = 1,
        Recommend = 2,
        Approve = 3,
        Return = 4,
        Reinitiate = 5,
        Refer = 6,
        ReferralApproved = 7,
        ForwardForReview = 8,
        RequestReviewed = 9,
        Note = 10,
        Reject = 11,
        Complete = 12,
        Forward = 13
    }

    public enum MemoTypeEnum
    {
        Blacklisting = 1,
        Deceased = 2,
        Reversal = 3
    }

    public enum UserRoleEnum
    {
        Administrator = 1,
        Authenticated = 2,
        CCAC = 3
    }
}
