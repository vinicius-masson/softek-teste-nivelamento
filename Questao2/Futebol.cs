using System.Text.Json.Serialization;

namespace Questao2
{
    public class Soccer
    {
        public string Competition { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Round { get; set; } = string.Empty;
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;


        [JsonPropertyName("team1goals")]
        public string Team1Goals { get; set; } = string.Empty;

        [JsonPropertyName("team2goals")]
        public string Team2Goals { get; set; } = string.Empty;
    }
}
