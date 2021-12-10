using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailService.Common.Email.Model;
using EmailService.Interfaces;
using Mailjet.Client;

namespace EmailService.Common.Email.EmailSender
{
    public abstract class EmailSender : IEmailSender
    {
        public static MailjetClient CreateMailJetClient()
        {
            return new MailjetClient("929fb53ed32ccba9202b593e08a69bb7", "f8da48cdc69404f4ad727bb96df82c54");
        }

        protected abstract Task Send(EmailModel email);

        public async Task SendMail(EmailModel emailModel)
        {
            await Send(emailModel);
        }

        public async Task SendMail(string adresse, string subject, string body, List<EmailAttachement>? emailAttachement = null)
        {
            await Send(new EmailModel
            {
                Attachements = emailAttachement,
                Body = body,
                EmailAdresse = adresse,
                Subject = subject
            });
        }

       
    } 
}
