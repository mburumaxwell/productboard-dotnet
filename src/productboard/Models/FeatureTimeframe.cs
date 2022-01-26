using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents the timeframe of a feature.</summary>
public record FeatureTimeframe
{
    /// <summary>Start of the timeframe. "none" means the value is not set.</summary>
    [JsonPropertyName("startDate")]
    public string? StartDate { get; set; }

    /// <summary>End of the timeframe. "none" means the value is not set.</summary>
    [JsonPropertyName("endDate")]
    public string? EndDate { get; set; }
}
