using System.Text.Json.Serialization;

namespace CrustStatistics
{
    public class RewardRequest
    {
        [JsonPropertyName("row")] public int Row { get; set; } = 100;

        [JsonPropertyName("page")] public int Page { get; set; } = 0;

        [JsonPropertyName("address")] public string Address { get; set; }
    }
}