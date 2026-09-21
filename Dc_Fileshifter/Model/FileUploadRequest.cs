using System.IO;

namespace Dc_Fileshifter.Model
{
    public class FileUploadRequest
    {
        public FileUploadRequest()
        {
        }
        public string FileName { get; set; }
        public string Folder { get; set; }
        public Stream File { get; set; }
    }
}

