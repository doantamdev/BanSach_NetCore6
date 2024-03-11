using System;
using System.Net;
using System.Net.Mail;
using WebBanSach.DesignPattern_Tam;

namespace WebBanSach.DesignPattern_Tam
{
    public class EmailLoggingDecorator
    {
        public bool SendEmail(string to, string subject, string body, string attachFile)
        {
            try
            {
                MailMessage msg = new MailMessage(ConstantHelper.emailSender, to, subject, body);
                using (var client = new SmtpClient(ConstantHelper.hostEmail, ConstantHelper.portEmail))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(ConstantHelper.emailSender, ConstantHelper.passwordSender);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Timeout = 10000; // Timeout set to 10 seconds

                    if (!string.IsNullOrEmpty(attachFile))
                    {
                        Attachment attachment = new Attachment(attachFile);
                        msg.Attachments.Add(attachment);
                    }

                    client.Send(msg);
                }
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý ngoại lệ
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
