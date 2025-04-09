namespace CryptoPortfolioApp.Components.Models
{
    public class PortfolioCalculationResult
    {
        public List<CoinCalculationResult> Coins { get; set; }

        public double InitialTotal { get; set; }

        public double CurrentTotal { get; set; }

        public double OverallChange { get; set; }

        public double OverallChangePercentage { get; set; }
    }
}
