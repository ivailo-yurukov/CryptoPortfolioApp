using CryptoPortfolioApp.Components.Interfaces;
using CryptoPortfolioApp.Components.Models;
using System.Globalization;

namespace CryptoPortfolioApp.Components.Services
{
    public class PortfolioCalculatorService : IPortfolioCalculatorService
    {
        private readonly CoinloreService _coinloreService;
        private readonly ILogger<PortfolioCalculatorService> _logger;

        public PortfolioCalculatorService(CoinloreService coinloreService, ILogger<PortfolioCalculatorService> logger)
        {
            _coinloreService = coinloreService;
            _logger = logger;
        }

        public async Task<PortfolioCalculationResult> CalculateAsync(List<PortfolioEntry> entries)
        {
            // Fetch the latest coin data from Coinlore.
            var coins = await _coinloreService.GetCoinsAsync();

            double initialTotal = 0;
            double currentTotal = 0;
            var coinResults = new List<CoinCalculationResult>();

            foreach (var entry in entries)
            {
                double initialValue = entry.Amount * entry.BuyPrice;
                initialTotal += initialValue;
              
                var coinData = coins.FirstOrDefault(c =>
                    string.Equals(c.Symbol, entry.Coin, StringComparison.OrdinalIgnoreCase));

                // If coin data exists parse its current price.
                double currentPrice = 0;
                if (coinData != null && double.TryParse(coinData.PriceUsd, NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
                {
                    currentPrice = price;
                }
                else
                {
                    _logger.LogWarning("No data found for coin: {Coin}", entry.Coin);
                }

                double currentValue = entry.Amount * currentPrice;
                currentTotal += currentValue;
                double percentageChange = entry.BuyPrice > 0 ? ((currentPrice - entry.BuyPrice) / entry.BuyPrice) * 100 : 0;

                coinResults.Add(new CoinCalculationResult
                {
                    Coin = entry.Coin,
                    Amount = entry.Amount,
                    BuyPrice = entry.BuyPrice,
                    InitialValue = initialValue,
                    CurrentPrice = currentPrice,
                    CurrentValue = currentValue,
                    PercentageChange = percentageChange
                });
            }

            double overallChange = initialTotal > 0 ? (currentTotal - initialTotal) : 0;
            double overallChangePercentage = initialTotal > 0 ? ((currentTotal - initialTotal) / initialTotal) * 100 : 0;

            return new PortfolioCalculationResult
            {
                Coins = coinResults,
                InitialTotal = initialTotal,
                CurrentTotal = currentTotal,
                OverallChange = overallChange,
                OverallChangePercentage = overallChangePercentage
            };
        }
    }
}
