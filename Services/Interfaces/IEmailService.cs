using PicturesAPI.Models;

namespace PicturesAPI.Services.Interfaces;

public interface IEmailService
{
    bool SendLogsAsEmail(EmailData emailData);
}