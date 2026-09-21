using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dc_Fileshifter.Model
{
        public class SharePointFolderResponse
        {
            [JsonProperty("d")]
            public SharePointFolder Folders { get; set; }
        }

        public class SharePointFolder
        {
            [JsonProperty("results")]
            public IReadOnlyList<SharePointResult> Results { get; set; }
        }

        public class SharePointResult
        {
            [JsonProperty("Exists")]
            public bool Exists { get; set; }

            [JsonProperty("ItemCount")]
            public int ItemCount { get; set; }

            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("ServerRelativeUrl")]
            public string ServerRelativeUrl { get; set; }
        }
    
}

