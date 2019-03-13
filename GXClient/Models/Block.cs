using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gxclient.Models
{
    public class Block
    {
        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("witness")]
        public string Witness { get; set; }

        [JsonProperty("transaction_merkle_root")]
        public string TransactionMerkleRoot { get; set; }

        [JsonProperty("extensions")]
        public object[] Extensions { get; set; }

        [JsonProperty("witness_signature")]
        public string WitnessSignature { get; set; }

        [JsonProperty("transactions")]
        public Transaction[] Transactions { get; set; }

        [JsonProperty("block_id")]
        public string BlockId { get; set; }

        [JsonProperty("signing_key")]
        public string SigningKey { get; set; }

        [JsonProperty("transaction_ids")]
        public string[] TransactionIds { get; set; }
    }
}
