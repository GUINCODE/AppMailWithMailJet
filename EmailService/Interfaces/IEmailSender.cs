 using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailService.Common.Email.Model;

namespace EmailService.Interfaces
{
    public interface IEmailSender
    {
        Task SendMail(string adresse, string subject, string body, List<EmailAttachement>? emailAttachement =null);
        Task SendMail(EmailModel emailModel);
    }
}
