using CryptoPortfolioApp.Components.Models;

namespace CryptoPortfolioApp.Components.Interfaces
{
    public interface ICoinloreService
    {
        Task<List<Coin>> GetCoinsAsync();
    }
}
