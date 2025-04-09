namespace CryptoPortfolioApp.Components.Models
{
    public class CoinCalculationResult
    {
        public string Coin { get; set; }
        public double Amount { get; set; }
        public double BuyPrice { get; set; }
        public double InitialValue { get; set; }
        public double CurrentPrice { get; set; }
        public double CurrentValue { get; set; }
        public double PercentageChange { get; set; }
    }
}
