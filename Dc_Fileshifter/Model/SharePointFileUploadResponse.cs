using System;
using Newtonsoft.Json;

namespace Dc_Fileshifter.Model
{
    public class SharePointFileUploadResponse
    {
        [JsonProperty("d")]
        public SharePointFileUploadResult Result { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }

    public class SharePointFileUploadResult
    {
        [JsonProperty("LinkingUri")]
        public string LinkingUri { get; set; }

        [JsonProperty("ServerRelativeUrl")]
        public string ServerRelativeUrl { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

    }
}

