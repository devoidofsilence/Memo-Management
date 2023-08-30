using Newtonsoft.Json;
using SBLApps.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using SBLApps.Enums;
using SBLApps.Helpers;

namespace SBLApps.Services
{
    public class CommonService
    {
        // Inject IConfiguration into your class constructor
        private readonly IConfiguration _configuration;
        public CommonService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void SendEmail(EmailDetail emailDetail)
        {
            // Retrieve SMTP settings from appsettings.json
            string smtpHost = _configuration.GetValue<string>("Smtp:Host");
            int smtpPort = _configuration.GetValue<int>("Smtp:Port");
            string smtpUsername = _configuration.GetValue<string>("Smtp:Username");
            string smtpPassword = _configuration.GetValue<string>("Smtp:Password");

            // Create a new MailMessage object
            MailMessage message = new MailMessage();
            message.From = new MailAddress(smtpUsername);
            message.To.Add(emailDetail.AssigneeEmail);
            switch (emailDetail.OperationType)
            {
                case (int)OperationEnum.Initiate:
                case (int)OperationEnum.Reinitiate:
                case (int)OperationEnum.Recommend:
                    EmailTemplateToRecommender emailToRecommender = new EmailTemplateToRecommender();
                    message.Subject = emailToRecommender.Subject;
                    message.Body = emailToRecommender.Body(emailDetail);
                    break;
                case (int)OperationEnum.Approve:
                    //ApprovedEmailTemplateToInitiator approvedEmailToInitiator = new ApprovedEmailTemplateToInitiator();
                    //message.Subject = approvedEmailToInitiator.Subject;
                    //message.Body = approvedEmailToInitiator.Body(emailDetail);
                    ApprovedNotedEmailTemplateToCCACUsers approvedEmailTemplateToCCACUsers = new ApprovedNotedEmailTemplateToCCACUsers();
                    message.Subject = approvedEmailTemplateToCCACUsers.Subject;
                    message.Body = approvedEmailTemplateToCCACUsers.Body(emailDetail);
                    break;
                case (int)OperationEnum.Note:
                    //NotedEmailTemplateToInitiator notedEmailToInitiator = new NotedEmailTemplateToInitiator();
                    //message.Subject = notedEmailToInitiator.Subject;
                    //message.Body = notedEmailToInitiator.Body(emailDetail);
                    ApprovedNotedEmailTemplateToCCACUsers notedEmailTemplateToCCACUsers = new ApprovedNotedEmailTemplateToCCACUsers();
                    message.Subject = notedEmailTemplateToCCACUsers.Subject;
                    message.Body = notedEmailTemplateToCCACUsers.Body(emailDetail);
                    break;
                case (int)OperationEnum.Reject:
                    RejectedEmailTemplateToInitiator rejectedEmailToInitiator = new RejectedEmailTemplateToInitiator();
                    message.Subject = rejectedEmailToInitiator.Subject;
                    message.Body = rejectedEmailToInitiator.Body(emailDetail);
                    break;
                case (int)OperationEnum.Return:
                    ReturnedEmailTemplateToInitiator returnedEmailToInitiator = new ReturnedEmailTemplateToInitiator();
                    message.Subject = returnedEmailToInitiator.Subject;
                    message.Body = returnedEmailToInitiator.Body(emailDetail);
                    break;
                case (int)OperationEnum.Refer:
                    ForReferEmailTemplateToReference forReferEmailTemplateToReference = new ForReferEmailTemplateToReference();
                    message.Subject = forReferEmailTemplateToReference.Subject;
                    message.Body = forReferEmailTemplateToReference.Body(emailDetail);
                    break;
                case (int)OperationEnum.ReferralApproved:
                    ReferredEmailTemplateToReferrer referredEmailTemplateToReferrer = new ReferredEmailTemplateToReferrer();
                    message.Subject = referredEmailTemplateToReferrer.Subject;
                    message.Body = referredEmailTemplateToReferrer.Body(emailDetail);
                    break;
                case (int)OperationEnum.ForwardForReview:
                    ForReviewEmailTemplateToReviewer forReviewEmailTemplateToReviewer = new ForReviewEmailTemplateToReviewer();
                    message.Subject = forReviewEmailTemplateToReviewer.Subject;
                    message.Body = forReviewEmailTemplateToReviewer.Body(emailDetail);
                    break;
                case (int)OperationEnum.RequestReviewed:
                    ReviewedEmailTemplateToRecommender reviewedEmailTemplateToRecommender = new ReviewedEmailTemplateToRecommender();
                    message.Subject = reviewedEmailTemplateToRecommender.Subject;
                    message.Body = reviewedEmailTemplateToRecommender.Body(emailDetail);
                    break;
                case (int)OperationEnum.Complete:
                    CompletedEmailTemplateToInitiator completedEmailToInitiator = new CompletedEmailTemplateToInitiator();
                    message.Subject = completedEmailToInitiator.Subject;
                    message.Body = completedEmailToInitiator.Body(emailDetail);
                    break;
                default:
                    break;
            }
            message.IsBodyHtml = true;

            // Create an SMTP client and send the email
            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = _configuration.GetValue<bool>("Smtp:EnableSsl");
                smtpClient.Send(message);
            }
        }
    }
}
