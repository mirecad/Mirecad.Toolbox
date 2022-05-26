using System;
using System.Collections.Generic;
using System.IO;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace Mirecad.Toolbox.Emails
{
    public class SmtpEmailSender : IDisposable, IEmailSender
    {
        private SmtpClient _smtpClient;
        private readonly string _senderAddress;
        private readonly object _lock = new object();
        private readonly string _username;
        private readonly string _pwd;
        private readonly string _server;
        private readonly int _port;

        public SmtpEmailSender(string server, int port, string username, string password)
        {
            var isAddressFormat = username.Contains("@");
            if (isAddressFormat == false)
            {
                throw new FormatException($"Username must be in email address format.");
            }

            _username = username;
            _pwd = password;
            _server = server;
            _port = port;
            _senderAddress = username;
            _smtpClient = new SmtpClient();
        }

        public void SendTextEmail(string toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            SendTextEmail(new List<string>() { toAddress }, subject, body, attachments);
        }

        public void SendTextEmail(IEnumerable<string> toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            var message = ConstructTextMimeMessage(toAddress, subject, body, attachments);
            SendMimeMessage(message);
        }

        public void SendHtmlEmail(string toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            SendHtmlEmail(new List<string>() { toAddress }, subject, body, attachments);
        }

        public void SendHtmlEmail(IEnumerable<string> toAddress, string subject, string body, IEnumerable<IEmailAttachment> attachments = null)
        {
            attachments ??= new List<IEmailAttachment>();
            var message = ConstructHtmlMimeMessage(toAddress, subject, body, attachments);
            SendMimeMessage(message);
        }

        private MimeMessage ConstructTextMimeMessage(IEnumerable<string> toAddress,
            string subject, string body, IEnumerable<IEmailAttachment> attachments)
        {
            var message = new MimeMessage();
            message = ConstructMessageHeaders(message, toAddress, subject);
            message = ConstructMessageBody(message, null, body, attachments);
            return message;
        }

        private MimeMessage ConstructHtmlMimeMessage(IEnumerable<string> toAddress,
            string subject, string body, IEnumerable<IEmailAttachment> attachments)
        {
            var message = new MimeMessage();
            message = ConstructMessageHeaders(message, toAddress, subject);
            message = ConstructMessageBody(message, body, null, attachments);
            return message;
        }

        private MimeMessage ConstructMessageHeaders(MimeMessage message, IEnumerable<string> toAddress, string subject)
        {
            message.From.Add(MailboxAddress.Parse(_senderAddress));
            foreach (var address in toAddress)
            {
                message.To.Add(MailboxAddress.Parse(address));
            }
            message.Subject = subject;
            return message;
        }

        private MimeMessage ConstructMessageBody(MimeMessage message, string htmlBody, string textBody, IEnumerable<IEmailAttachment> attachments)
        {
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;
            builder.TextBody = textBody;
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    switch (attachment)
                    {
                        case FilepathEmailAttachment att:
                            var filename = Path.GetFileName(att.Path);
                            var content = File.ReadAllBytes(att.Path);
                            builder.Attachments.Add(filename, content);
                            break;
                        case ByteEmailAttachment att:
                            builder.Attachments.Add(att.Filename, att.Content);
                            break;
                        default:
                            throw new InvalidOperationException("Unknown attachment type.");
                    }
                }
            }
            message.Body = builder.ToMessageBody();
            return message;
        }

        private void SendMimeMessage(MimeMessage message)
        {
            lock (_lock)
            {
                try
                {
                    _smtpClient.Send(message);
                    return;
                }
                catch (ServiceNotConnectedException)
                {
                    Connect();
                }
                catch (SmtpProtocolException)
                {
                    if (_smtpClient.IsConnected == false)
                    {
                        _smtpClient.Dispose();
                        Connect();
                    }
                }
                _smtpClient.Send(message);
            }
        }

        private void Connect()
        {
            _smtpClient = new SmtpClient();
            _smtpClient.Connect(_server, _port);
            _smtpClient.Authenticate(_username, _pwd);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (_lock)
                {
                    if (_smtpClient != null)
                    {
                        _smtpClient.Disconnect(true);
                        _smtpClient?.Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
