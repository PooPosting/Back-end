using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Options;
using MimeKit;
using PicturesAPI.Configuration;
using PicturesAPI.Models;
using PicturesAPI.Services.Interfaces;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PicturesAPI.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    public EmailService(IConfiguration config)
    {
        _emailSettings = new EmailSettings()
        {
            SendLogsToId = config.GetValue<string>("EmailSettings:SendLogsToId"),
            SendLogsToName = config.GetValue<string>("EmailSettings:SendLogsToName"),
            Name = config.GetValue<string>("EmailSettings:Name"),
            EmailId = config.GetValue<string>("EmailSettings:EmailId"),
            Host = config.GetValue<string>("EmailSettings:Host"),
            Password = config.GetValue<string>("EmailSettings:Password"),
            Port = config.GetValue<int>("EmailSettings:Port"),
            UseSsl = config.GetValue<bool>("EmailSettings:UseSSL"),
        };
    }
    public bool SendLogsAsEmail(EmailData emailData)
    {
        try
        {
            var emailMessage = new MimeMessage();
            var emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
            emailMessage.From.Add(emailFrom);
            var emailTo = new MailboxAddress(_emailSettings.SendLogsToName, _emailSettings.SendLogsToId);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = emailData.EmailSubject;
            
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = emailData.EmailBody;

            if (emailData.EmailJsonLog != "")
            {
                var bytes = Encoding.ASCII.GetBytes(emailData.EmailJsonLog); 
                bodyBuilder.Attachments.Add($"ErrorLog-{DateTime.Now.ToLocalTime()}.json", bytes);
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            
            var emailClient = new SmtpClient();
            emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSsl);
            emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
            emailClient.Send(emailMessage);
            emailClient.Disconnect(true);
            emailClient.Dispose();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}