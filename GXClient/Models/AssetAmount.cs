using System;
using Newtonsoft.Json;
namespace gxclient.Models
{
    public class AssetAmount
    {
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }
        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
