using System;
using Newtonsoft.Json;

namespace Dc_Fileshifter.Model
{
    public class DC_Datastore
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("file")]
        public FileInfo File { get; set; }
    }

    public class FileInfo
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }
        [JsonProperty("filePath")]
        public string FilePath { get; set; }
        [JsonProperty("fileStatus")]
        public string FileStatus { get; set; }
        [JsonProperty("sourceSystem")]
        public string SourceSystem { get; set; }
        [JsonProperty("scanStatus")]
        public string ScanStatus { get; set; }
    }
}

