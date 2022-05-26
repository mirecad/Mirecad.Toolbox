using System;

namespace Mirecad.Toolbox.Emails
{
    public class FilepathEmailAttachment : IEmailAttachment
    {
        public FilepathEmailAttachment(string path)
        {
            Path = path ?? throw new ArgumentException($"{nameof(path)} cannot be empty.");
        }

        public string Path { get; }
    }
}