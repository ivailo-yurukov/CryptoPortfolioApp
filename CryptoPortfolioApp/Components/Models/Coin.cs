using System.Text.Json.Serialization;

namespace CryptoPortfolioApp.Components.Models
{
    public class Coin
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Example property – the API returns price as a string; adjust if necessary.
        [JsonPropertyName("price_usd")]
        public string PriceUsd { get; set; }
    }
}
