using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs
{
    public class MailDTO
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public MailDTO(string emailFrom, string emailTo, string subject, string body)
        {
            EmailFrom = emailFrom;
            EmailTo = emailTo;
            Subject = subject;
            Body = body;
        }
    }
}
