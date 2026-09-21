using System;
using Newtonsoft.Json;

namespace Dc_Fileshifter.Model
{
    public class SharePointContext
    {
        [JsonProperty("d")]
        public SharePointContextResponse Response { get; set; }
    }

    public class SharePointContextResponse
    {
        [JsonProperty("GetContextWebInformation")]
        public GetContextWebInformation GetContextWebInformation { get; set; }
    }


    public class GetContextWebInformation
    {
        [JsonProperty("FormDigestValue")]
        public string FormDigestValue { get; set; }
    }
}

