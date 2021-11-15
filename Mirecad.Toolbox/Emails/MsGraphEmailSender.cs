using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Mirecad.Toolbox.Emails
{
    public class MsGraphEmailSender : IEmailSender
    {
        private readonly GraphServiceClient _client;
        private readonly string _sender;

        public MsGraphEmailSender(string sender, GraphServiceClient client)
        {
            _client = client;
            _sender = sender;
        }

        public void SendTextEmail(string toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            SendTextEmail(new List<string>() { toAddress }, subject, body, attachments);
        }

        public void SendTextEmail(IEnumerable<string> toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            SendEmail(email => email.To(toAddress)
                .Subject(subject)
                .Body(body, BodyType.Text)
                .Attachments(attachments));
        }

        public void SendHtmlEmail(string toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            SendHtmlEmail(new List<string>() { toAddress }, subject, body, attachments);
        }

        public void SendHtmlEmail(IEnumerable<string> toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            SendEmail(email => email.To(toAddress)
                .Subject(subject)
                .Body(body, BodyType.Html)
                .Attachments(attachments));
        }

        private void SendEmail(Action<EmailBuilder> builder)
        {
            var message = new Message();
            builder(new EmailBuilder(message));
            SendEmailInternalAsync(message).Wait();
        }

        private async Task SendEmailInternalAsync(Message message)
        {
            await _client.Users[_sender]
                .SendMail(message)
                .Request()
                .PostAsync();
        }
    }
}