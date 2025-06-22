using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO.IdentityDTOS;
using Ecom.Core.ServicesContract;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Ecom.infrastructure.Repository.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Afify-Tech",configuration["EmailSetting:From"])); 
            message.Subject = emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };
            using(var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(configuration["EmailSetting:Smtp"],
                                            int.Parse(configuration["EmailSetting:Port"]), SecureSocketOptions.SslOnConnect);
                    await smtp.AuthenticateAsync(configuration["EmailSetting:Username"], configuration["EmailSetting:Password"]);
                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("⛔ SMTP ERROR: " + ex.Message);
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
