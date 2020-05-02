using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Common.Core.Entities.File
{
    public class UploadedFile
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
                SourceExtension = Path.GetExtension(file.FileName)?.Trim('.'),
                Content = content,
                Hash = hash
            };
        }
    }
}