using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace site.Data
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _emailSetting;
        private readonly ILogger<EmailSender> logger;

        public EmailSender(IOptions<EmailSetting> emailSetting, ILogger<EmailSender> logger)
        {
            _emailSetting = emailSetting.Value;
            this.logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
           using(var client = new SmtpClient())
            {
                client.Port = _emailSetting.Port;
                client.Host = _emailSetting.Smtp;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password);
                var message = new MailMessage
                {
                    From = new MailAddress("noreply@shopatfirstchoice.com"),
                    Sender = new MailAddress("noreply@shopatfirstchoice.com"),
                    IsBodyHtml = true,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    Subject = subject,
                    Body = htmlMessage                   
                };

                message.To.Add(email);

                client.Send(message);

                logger.LogWarning(">>> Finished sending mail");
                await Task.CompletedTask;
            }
        }

        public static void EmailSent() { }
    }

    public class EmailSetting
    {
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
