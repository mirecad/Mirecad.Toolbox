using System.Collections.Generic;

namespace Mirecad.Toolbox.Emails
{
    public interface IEmailSender
    {
        void SendTextEmail(string toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null);
        void SendTextEmail(IEnumerable<string> toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null);
        void SendHtmlEmail(string toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null);
        void SendHtmlEmail(IEnumerable<string> toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null);
    }
}