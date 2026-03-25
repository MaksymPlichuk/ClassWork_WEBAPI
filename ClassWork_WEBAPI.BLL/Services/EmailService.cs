using ClassWork_WEBAPI.BLL.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.BLL.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _settings = smtpOptions.Value;

            _smtpClient = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_settings.Email, _settings.Password)
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(_settings.Email)
                };
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
