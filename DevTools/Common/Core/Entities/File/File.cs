using System;
using System.Text;

namespace VXDesign.Store.DevTools.Common.Core.Entities.File
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FileExtension Extension { get; set; }
        public byte[] ByteContent { get; set; }
        public string Content => Encoding.Unicode.GetString(ByteContent);
        public string Hash { get; set; }
        public DateTime Time { get; set; }
    }
}