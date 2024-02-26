using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSach.Models
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(string to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.Add(new MailboxAddress("email", to));
            Subject = subject;
            Content = content;
        }
    }

}
