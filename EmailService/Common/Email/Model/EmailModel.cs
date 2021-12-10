using System.Collections.Generic;

namespace EmailService.Common.Email.Model
{
    public class EmailModel
    {

        public string EmailAdresse { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public IEnumerable<EmailAttachement>? Attachements  { get; set; }

       
    }

    public class EmailAttachement
    {
        public string Name { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public byte[] Data = new byte[0];

    }
}
