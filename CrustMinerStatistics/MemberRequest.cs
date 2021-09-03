using System.Text.Json.Serialization;

namespace CrustStatistics
{
    public class MemberRequest
    {
        [JsonPropertyName("page")] public int Page { get; set; } = 0;
        [JsonPropertyName("row")] public int Row { get; set; } = 1000;
        [JsonPropertyName("group_owner")] public string GroupOwner { get; set; }
    }
}