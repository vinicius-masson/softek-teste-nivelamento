using System.Text.Json.Serialization;


namespace Questao2
{
    public class ApiResponse
    {
        public int Page { get; set; }
        public int Total_Pages { get; set; }

        [JsonPropertyName("data")]
        public List<Soccer> Data { get; set; }
    }
}
