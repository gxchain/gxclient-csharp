using System;
using Newtonsoft.Json;

namespace gxclient.Models
{
    public class KeyPair
    {
        [JsonProperty("private_key")]
        public string PrivateKey { get; set; }
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        [JsonProperty("brain_key")]
        public string BrainKey { get; set; }
    }
}
