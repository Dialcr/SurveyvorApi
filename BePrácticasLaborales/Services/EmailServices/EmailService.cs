using System.Net;
using System.Net.Mail;
using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BePrácticasLaborales.Services.EmailServices;

public class EmailService 
{
    private readonly SmtpClient _smtpClient;
    private readonly MailSettings _mailSettings;

    public EmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
        _smtpClient = new SmtpClient(mailSettings.Value.Smtp, mailSettings.Value.Port);
        _smtpClient.Credentials = new NetworkCredential(mailSettings.Value.From, mailSettings.Value.Password);
        _smtpClient.EnableSsl = true;
    }

    
    public void SendEmail(MailMessage mailMessage, string userToken)
    {
        _smtpClient.SendAsync(mailMessage, userToken);
    }
}