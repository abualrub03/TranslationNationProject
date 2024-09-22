using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace TranslationNation.Web.Models
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService()
        {
        }

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body , int type)
        {
            if (type == 1)
            {
                // Configure API key for Sendinblue
                Configuration.Default.ApiKey["api-key"] = "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";

                var apiInstance = new TransactionalEmailsApi();

                // Sender information
                string SenderName = "Translation Nation Team";
                string SenderEmail = "TranslationNation-Commettee@translationnationhub.com";
                SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

                // Recipient information
                string ToEmail = toEmail;
                string ToName = "Our New Translator";
                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
                List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo> { smtpEmailTo };

                // Subject and email content
                string Subject = "Welcome to Translation Nation - Account Pending Verification";
                string HtmlContent = $@"
        <html>
        <body>
            <h2>Welcome to Translation Nation, {ToName}!</h2>
            <p>We are excited to have you as a new member of our translator community.</p>
            <p>Your account is currently pending verification by your university supervisor.</p>
            <p>Once verified, you will be able to access our platform and start accepting translation requests.</p>
            <p>If you have any questions, feel free to reach out to us at support@translationnationhub.com.</p>
            <br>
            <p>Best regards,</p>
            <p>The Translation Nation Team</p>
        </body>
        </html>";
                string TextContent = "Welcome to Translation Nation, Your account is pending university verification.";

                try
                {
                    // Create and send the email via Sendinblue API
                    var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                    CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                    Console.WriteLine(result.ToJson());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                // Additionally, using SMTP client for local sending (if applicable)
                var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                    Credentials = new NetworkCredential(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderPassword"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:SenderEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
            
            
            else if(type == 2)
            {
                // Configure API key for Sendinblue
                Configuration.Default.ApiKey["api-key"] = "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";

                var apiInstance = new TransactionalEmailsApi();

                // Sender information
                string SenderName = "University Supervisor";
                string SenderEmail = "supervisor@translationnationhub.com";
                SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

                // Recipient information
                string ToEmail = toEmail;  // Replace with the student's email
                string ToName = "Invitation";
                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
                List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo> { smtpEmailTo };

                // Subject and email content
                string Subject = "Invitation to Join Translation Nation";
                string HtmlContent = $@"
<html>
<body>
    <h2>Hello {body},</h2>
    <p>You have been invited by your university supervisor to join <strong>Translation Nation</strong>, our community of translators.</p>
    <p>Please click the link below to create your account and start your journey as a translator with us:</p>
    <p><a href='https://translationnationhub.com/Customer/SignUp'>Join Translation Nation</a></p>
    <p>We are excited to welcome you!</p>
    <br>
    <p>Best regards,</p>
    <p>Your University Supervisor</p>
</body>
</html>";
                string TextContent = "Hello, You have been invited to join Translation Nation. Please visit the following link to register: https://www.translationnationhub.com/register";

                try
                {
                    // Create and send the email via Sendinblue API
                    var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                    CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                    Console.WriteLine(result.ToJson());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

              
            }
            else if (type == 3)
            {
                {
                    // Configure API key for Sendinblue
                    Configuration.Default.ApiKey["api-key"] = "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";

                    var apiInstance = new TransactionalEmailsApi();

                    // Sender information
                    string SenderName = "University Supervisor";
                    string SenderEmail = "supervisor@translationnationhub.com";
                    SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

                    // Recipient information
                    string ToEmail = toEmail;  // Replace with the student's email
                    string ToName = "Congratulations";
                    SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
                    List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo> { smtpEmailTo };

                    // Subject and email content
                    string Subject = "Congratulations! Your Email is Verified";
                    string HtmlContent = $@"
<html>
<body>
    <h2>Dear {body},</h2>
    <p>We are pleased to inform you that your email has been successfully verified by your university supervisor.</p>
    <p>Congratulations on this milestone! You are now part of <strong>Translation Nation</strong>, our community of translators.</p>
    <p>Click the link below to log in and start exploring exciting opportunities:</p>
    <p><a href='https://translationnationhub.com/Customer/SignIn'>Log In to Translation Nation</a></p>
    <p>We look forward to seeing your contributions as a translator!</p>
    <br>
    <p>Best regards,</p>
    <p>Your University Supervisor</p>
</body>
</html>";
                    string TextContent = "Dear Student, Congratulations! Your email has been verified by your university supervisor. You can now log in to Translation Nation: https://www.translationnationhub.com/login";

                    try
                    {
                        // Create and send the email via Sendinblue API
                        var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                        CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                        Console.WriteLine(result.ToJson());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }

        }
    }
}
