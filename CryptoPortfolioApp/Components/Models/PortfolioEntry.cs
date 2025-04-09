namespace CryptoPortfolioApp.Components.Models
{
    public class PortfolioEntry
    {
        // Coin format entry X|COIN|Y

        // X
        public double Amount { get; set; }
        // Coin name
        public string Coin { get; set; }
        // Y
        public double BuyPrice { get; set; }
    }
}
