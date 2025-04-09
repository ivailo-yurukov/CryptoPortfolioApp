using CryptoPortfolioApp.Components.Models;

namespace CryptoPortfolioApp.Components.Interfaces
{
    public interface IPortfolioCalculatorService
    {
        Task<PortfolioCalculationResult> CalculateAsync(List<PortfolioEntry> portfolioEntries);
    }
}
