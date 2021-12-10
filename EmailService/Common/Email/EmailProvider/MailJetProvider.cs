using System;
using System.Linq;
using System.Threading.Tasks;
using EmailService.Common.Email.Model;
using EmailService.Interfaces;
using Mailjet.Client;
using Newtonsoft.Json.Linq;

namespace EmailService.Common.Email.EmailProvider
{
    public class MailJetProvider : EmailSender.EmailSender, IEmailSender
    {
        protected override async Task Send(EmailModel email)
        {
            try
            {
                JArray jArray = new JArray();
                JArray attachements = new JArray();

                if(email.Attachements != null && email.Attachements.Count() > 0)
                {
                    email.Attachements.ToList().ForEach(attachement => attachements.Add(
                        new JObject
                        {
                            new JProperty("Content-Type", attachement.ContentType),
                            new JProperty("Filename", attachement.Name),
                            new JProperty("Content-Type", attachement.ContentType),
                            new JProperty("Content",Convert.ToBase64String(attachement.Data))
                        }));

                }
                jArray.Add(new JObject
                {
                    new JProperty("fromEmail", "ageeb007@gmail.com"), //le meme mail que celui avec lequel on a creer le compte mailJet
                    new JProperty("FromName", "PointBase"),
                    new JProperty("Recipients", new JArray
                    {
                        new JObject
                        {
                            new JProperty("Email", email.EmailAdresse),
                             new JProperty("Name", email.EmailAdresse),
                        }
                    }),
                    new JProperty("subject", email.Subject),
                    new JProperty("Text-part", email.Body),
                    new JProperty("Html-part", email.Body), //pour utiliser le formet html
                    new JProperty("Attachements", attachements),
                     
                });

                var client = EmailSender.EmailSender.CreateMailJetClient();
                var request = new MailjetRequest
                {
                    Resource = Mailjet.Client.Resources.Send.Resource
                }
                .Property(Mailjet.Client.Resources.Send.Messages, jArray);
                var response = await client.PostAsync(request);
                Console.WriteLine($"Send result {response.StatusCode}, message : {response.Content}");

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}