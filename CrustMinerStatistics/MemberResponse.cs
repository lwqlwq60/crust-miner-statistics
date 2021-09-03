using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrustStatistics
{
    public class MemberResponse
    {
        [JsonPropertyName("code")] public int Code { get; set; }
        [JsonPropertyName("data")] public MemberDataList Data { get; set; }
    }


    public class MemberDataList
    {
        [JsonPropertyName("count")] public int Count { get; set; }
        [JsonPropertyName("list")] public IEnumerable<MemberData> List { get; set; }
    }

    public class MemberData
    {
        public string Owner { get; set; }
        [JsonPropertyName("account_id")] public string AccountId { get; set; }
        [JsonPropertyName("cap")] public ulong Capacity { get; set; }
    }
}