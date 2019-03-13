using System;
using Newtonsoft.Json;
namespace gxclient.Models
{
    public partial class Asset
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("precision")]
        public long Precision { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("options")]
        public AssetOptions Options { get; set; }

        [JsonProperty("dynamic_asset_data_id")]
        public string DynamicAssetDataId { get; set; }

        [JsonProperty("detail")]
        public AssetDetail Detail { get; set; }
    }

    public partial class AssetDetail
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("current_supply")]
        public string CurrentSupply { get; set; }

        [JsonProperty("confidential_supply")]
        public long ConfidentialSupply { get; set; }

        [JsonProperty("accumulated_fees")]
        public long AccumulatedFees { get; set; }

        [JsonProperty("fee_pool")]
        public long FeePool { get; set; }
    }

    public partial class AssetOptions
    {
        [JsonProperty("max_supply")]
        public string MaxSupply { get; set; }

        [JsonProperty("market_fee_percent")]
        public long MarketFeePercent { get; set; }

        [JsonProperty("max_market_fee")]
        public long MaxMarketFee { get; set; }

        [JsonProperty("issuer_permissions")]
        public long IssuerPermissions { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }

        [JsonProperty("core_exchange_rate")]
        public CoreExchangeRate CoreExchangeRate { get; set; }

        [JsonProperty("whitelist_authorities")]
        public object[] WhitelistAuthorities { get; set; }

        [JsonProperty("blacklist_authorities")]
        public object[] BlacklistAuthorities { get; set; }

        [JsonProperty("whitelist_markets")]
        public object[] WhitelistMarkets { get; set; }

        [JsonProperty("blacklist_markets")]
        public object[] BlacklistMarkets { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("extensions")]
        public object[] Extensions { get; set; }
    }

    public partial class CoreExchangeRate
    {
        [JsonProperty("base")]
        public AssetAmount Base { get; set; }

        [JsonProperty("quote")]
        public AssetAmount Quote { get; set; }
    }
}
