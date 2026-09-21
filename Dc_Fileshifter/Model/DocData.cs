
using Newtonsoft.Json;

namespace doc_capture_api.Models
{
    public class DocData
    {
        public string? DCDID { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string fileStatus { get; set; }
        public string sourceSystem { get; set; }
        public string scanStatus { get; set; }

        public string fileId { get; set; }

    }
}
