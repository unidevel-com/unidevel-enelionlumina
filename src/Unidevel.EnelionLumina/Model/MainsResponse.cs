using System.Text.Json.Serialization;

namespace Unidevel.EnelionLumina.Model
{
    class MainsResponse
    {
        [JsonPropertyName("current_limit")] public int CurrentLimitAmp { get; set; }
        [JsonPropertyName("phases_limit")] public Phases PhasesLimit { get; set; }
        [JsonPropertyName("voltages")] public float[]? Voltages { get; set; }
    }
}
