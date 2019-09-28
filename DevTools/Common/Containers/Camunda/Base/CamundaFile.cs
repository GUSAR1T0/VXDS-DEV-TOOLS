using System.IO;
using VXDesign.Store.DevTools.Common.Extensions.Camunda;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Base
{
    public class CamundaFile
    {
        public byte[] Data { get; }
        public string FileName { get; }
        public string MimeType { get; }
        public string Encoding { get; }

        public CamundaFile(string fullPath)
        {
            Data = File.ReadAllBytes(fullPath);
            var fileInfo = new FileInfo(fullPath);
            FileName = fileInfo.Name;
            MimeType = fileInfo.GetMimeType();
            Encoding = fileInfo.GetEncoding();
        }

        internal CamundaFile(byte[] data, string fileName, string mimeType, string encoding)
        {
            Data = data;
            FileName = fileName;
            MimeType = mimeType;
            Encoding = encoding;
        }
    }
}