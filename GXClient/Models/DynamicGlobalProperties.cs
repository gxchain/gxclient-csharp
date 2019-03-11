using System;
using Newtonsoft.Json;
namespace gxclient.Models
{
    public class DynamicGlobalProperties
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("head_block_number")]
        public long HeadBlockNumber { get; set; }

        [JsonProperty("head_block_id")]
        public string HeadBlockId { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("current_witness")]
        public string CurrentWitness { get; set; }

        [JsonProperty("next_maintenance_time")]
        public DateTime NextMaintenanceTime { get; set; }

        [JsonProperty("last_budget_time")]
        public DateTime LastBudgetTime { get; set; }

        [JsonProperty("witness_budget")]
        public long WitnessBudget { get; set; }

        [JsonProperty("accounts_registered_this_interval")]
        public long AccountsRegisteredThisInterval { get; set; }

        [JsonProperty("recently_missed_count")]
        public long RecentlyMissedCount { get; set; }

        [JsonProperty("current_aslot")]
        public long CurrentAslot { get; set; }

        [JsonProperty("recent_slots_filled")]
        public string RecentSlotsFilled { get; set; }

        [JsonProperty("dynamic_flags")]
        public long DynamicFlags { get; set; }

        [JsonProperty("last_irreversible_block_num")]
        public long LastIrreversibleBlockNum { get; set; }
    }
}
