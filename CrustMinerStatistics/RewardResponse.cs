using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrustStatistics
{
    public class Pair
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("value")] public string Value { get; set; }
    }

    public class Reward
    {
        [JsonPropertyName("account")] public string Account { get; set; }

        [JsonPropertyName("amount")] public string Amount { get; set; }

        [JsonPropertyName("block_num")] public int BlockNum { get; set; }

        [JsonPropertyName("block_timestamp")] public int BlockTimestamp { get; set; }

        [JsonPropertyName("event_id")] public string EventId { get; set; }

        [JsonPropertyName("event_idx")] public int EventIdx { get; set; }

        [JsonPropertyName("event_index")] public string EventIndex { get; set; }

        [JsonPropertyName("extrinsic_hash")] public string ExtrinsicHash { get; set; }

        [JsonPropertyName("extrinsic_idx")] public int ExtrinsicIdx { get; set; }

        [JsonPropertyName("extrinsic_index")] public string ExtrinsicIndex { get; set; }

        [JsonPropertyName("module_id")] public string ModuleId { get; set; }

        [JsonPropertyName("params")] public string Params { get; set; }

        [JsonPropertyName("stash")] public string Stash { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("count")] public int Count { get; set; }

        [JsonPropertyName("list")] public List<Reward> List { get; set; }
    }

    public class RewardResponse
    {
        [JsonPropertyName("code")] public int Code { get; set; }

        [JsonPropertyName("message")] public string Message { get; set; }

        [JsonPropertyName("generated_at")] public int GeneratedAt { get; set; }

        [JsonPropertyName("data")] public Data Data { get; set; }
    }
}