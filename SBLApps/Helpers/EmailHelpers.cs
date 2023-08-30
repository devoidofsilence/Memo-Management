using SBLApps.Models;

namespace SBLApps.Helpers
{
    public class EmailTemplateToRecommender
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been forwarded keeping you as the recommender by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class ReturnedEmailTemplateToInitiator
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been returned back to you by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class RejectedEmailTemplateToInitiator
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been rejected by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class CompletedEmailTemplateToInitiator
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been completed by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class NotedEmailTemplateToInitiator
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been noted by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class ForReferEmailTemplateToReference
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been assigned to you for reference by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class ReferredEmailTemplateToReferrer
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been referred and assigned to you by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class ForReviewEmailTemplateToReviewer
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been assigned to you for review by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class ReviewedEmailTemplateToRecommender
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been reviewed and assigned to you by: <b>{emailDetail.AssignerFullName}</b>. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }

    public class ApprovedNotedEmailTemplateToCCACUsers
    {
        public string Subject = "Details for Memo Request";

        public string Body(EmailDetail emailDetail)
        {
            string message;

            message = @$"<html><meta charset='UTF-8'><p><body dir='ltr'>
                            A Memo Request with reference no. <b>{emailDetail.MemoReferenceNumber}</b> has been approved and now forwarded to CCAC Department. To check the details, <a href='{emailDetail.RedirectUrl}'>Click Here</a>.<br><br>
                            </body></p></meta></html>";

            return message;
        }
    }
}
