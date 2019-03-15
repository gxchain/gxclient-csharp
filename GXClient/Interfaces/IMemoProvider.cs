using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace gxclient.Interfaces
{
    public class Memo
    {
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("to")]
        public string To { get; set; }
        [JsonProperty("nonce")]
        public UInt64 Nonce { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
    public interface IMemoProvider
    {
        /// <summary>
        /// Generate AEIES encrypted memo
        /// </summary>
        /// <returns>The memo.</returns>
        /// <param name="publicKey">Public key.</param>
        /// <param name="nonce">Nonce.</param>
        /// <param name="message">Message.</param>
        Memo GenerateMemo(string publicKey, UInt64 nonce, string message);
    }
}
