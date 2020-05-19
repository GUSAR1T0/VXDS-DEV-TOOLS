using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Common.Core.Entities.File
{
    public interface IFile
    {
        public string Name { get; }
        public string SourceExtension { get; }
        public FileExtension? Extension { get; }
        public string Content { get; }
        public string Hash { get; }
    }

    public class File : IFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SourceExtension { get; set; }
        public FileExtension? Extension { get; set; }
        public byte[] ByteContent { get; set; }
        public string Content => Encoding.Unicode.GetString(ByteContent);
        public string Hash { get; set; }
        public DateTime Time { get; set; }
    }

    public class UploadedFile : IFile
    {
        public string Name { get; private set; }
        public string SourceExtension { get; private set; }
        public FileExtension? Extension => EntityExtensions.DefineExtension(SourceExtension);
        public string Content { get; private set; }
        public string Hash { get; private set; }

        private UploadedFile()
        {
        }

        public static UploadedFile Transform(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var sha1 = SHA1.Create();

            var content = reader.ReadToEnd();
            var hash = BitConverter.ToString(sha1.ComputeHash(Encoding.Unicode.GetBytes(content))).Replace("-", string.Empty).ToLower();

            return new UploadedFile
            {
                Name = file.FileName,
                SourceExtension = Path.GetExtension(file.FileName)?.Trim('.').Trim().ToLower(),
                Content = content,
                Hash = hash
            };
        }
    }

    public class LocalFile
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }
}