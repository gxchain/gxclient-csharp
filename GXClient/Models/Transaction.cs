using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gxclient.Models
{
    public class Transaction
    {
        [JsonProperty("ref_block_num")]
        public long RefBlockNum { get; set; }

        [JsonProperty("ref_block_prefix")]
        public long RefBlockPrefix { get; set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }

        [JsonProperty("operations")]
        public JArray[] Operations { get; set; }

        [JsonProperty("extensions")]
        public object[] Extensions { get; set; }

        [JsonProperty("signatures")]
        public string[] Signatures { get; set; }
    }
}
