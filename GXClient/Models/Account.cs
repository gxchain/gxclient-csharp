using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gxclient.Models
{
    public class Account
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("membership_expiration_date")]
        public DateTimeOffset MembershipExpirationDate { get; set; }

        [JsonProperty("merchant_expiration_date")]
        public DateTimeOffset MerchantExpirationDate { get; set; }

        [JsonProperty("datasource_expiration_date")]
        public DateTimeOffset DatasourceExpirationDate { get; set; }

        [JsonProperty("data_transaction_member_expiration_date")]
        public DateTimeOffset DataTransactionMemberExpirationDate { get; set; }

        [JsonProperty("registrar")]
        public string Registrar { get; set; }

        [JsonProperty("referrer")]
        public string Referrer { get; set; }

        [JsonProperty("lifetime_referrer")]
        public string LifetimeReferrer { get; set; }

        [JsonProperty("merchant_auth_referrer")]
        public string MerchantAuthReferrer { get; set; }

        [JsonProperty("datasource_auth_referrer")]
        public string DatasourceAuthReferrer { get; set; }

        [JsonProperty("network_fee_percentage")]
        public long NetworkFeePercentage { get; set; }

        [JsonProperty("lifetime_referrer_fee_percentage")]
        public long LifetimeReferrerFeePercentage { get; set; }

        [JsonProperty("referrer_rewards_percentage")]
        public long ReferrerRewardsPercentage { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vm_type")]
        public string VmType { get; set; }

        [JsonProperty("vm_version")]
        public string VmVersion { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }

        [JsonProperty("abi")]
        public Abi Abi { get; set; }

        [JsonProperty("owner")]
        public Active Owner { get; set; }

        [JsonProperty("active")]
        public Active Active { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }

        [JsonProperty("statistics")]
        public string Statistics { get; set; }

        [JsonProperty("whitelisting_accounts")]
        public object[] WhitelistingAccounts { get; set; }

        [JsonProperty("blacklisting_accounts")]
        public object[] BlacklistingAccounts { get; set; }

        [JsonProperty("whitelisted_accounts")]
        public object[] WhitelistedAccounts { get; set; }

        [JsonProperty("blacklisted_accounts")]
        public object[] BlacklistedAccounts { get; set; }
    }

    public partial class Abi
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("types")]
        public object[] Types { get; set; }

        [JsonProperty("structs")]
        public object[] Structs { get; set; }

        [JsonProperty("actions")]
        public object[] Actions { get; set; }

        [JsonProperty("tables")]
        public object[] Tables { get; set; }

        [JsonProperty("error_messages")]
        public object[] ErrorMessages { get; set; }

        [JsonProperty("abi_extensions")]
        public object[] AbiExtensions { get; set; }
    }

    public partial class Active
    {
        [JsonProperty("weight_threshold")]
        public long WeightThreshold { get; set; }

        [JsonProperty("account_auths")]
        public JArray[] AccountAuths { get; set; }

        [JsonProperty("key_auths")]
        public JArray[] KeyAuths { get; set; }

        [JsonProperty("address_auths")]
        public JArray[] AddressAuths { get; set; }
    }

    public partial class Options
    {
        [JsonProperty("memo_key")]
        public string MemoKey { get; set; }

        [JsonProperty("voting_account")]
        public string VotingAccount { get; set; }

        [JsonProperty("num_witness")]
        public long NumWitness { get; set; }

        [JsonProperty("num_committee")]
        public long NumCommittee { get; set; }

        [JsonProperty("votes")]
        public string[] Votes { get; set; }

        [JsonProperty("extensions")]
        public JArray Extensions { get; set; }
    }
}
