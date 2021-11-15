namespace Mirecad.Toolbox.Emails
{
    public class ByteEmailAttachment : IEmailAttachment
    {
        public string Filename { get; set; }
        public byte[] Content { get; set; }
    }
}