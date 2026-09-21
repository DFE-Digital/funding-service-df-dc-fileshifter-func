using System;
using Newtonsoft.Json;

namespace Dc_Fileshifter.Model
{
    public class FileDetail
    {
        public FileDetail()
        {
        }
        [JsonProperty("fileId")]
        public Guid FileId { get; set; }
        [JsonProperty("sourceSystem")]
        public string SourceSystem { get; set; }
        [JsonProperty("targetRootFolder")]
        public string TargetRootFolder { get; set; }
        [JsonProperty("targetFilePath")]
        public string TargetFilePath { get; set; }
    }
}

