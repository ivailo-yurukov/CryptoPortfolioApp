using CryptoPortfolioApp.Components.Interfaces;
using CryptoPortfolioApp.Components.Models;

namespace CryptoPortfolioApp.Components.Services
{
    public class CoinloreService : ICoinloreService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoinloreService> _logger;

        public CoinloreService(HttpClient httpClient, ILogger<CoinloreService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Coin>> GetCoinsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CoinApiResponse>("https://api.coinlore.net/api/tickers/");
                return response?.Data ?? new List<Coin>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching coin data: {0}", ex.Message);
                return new List<Coin>();
            }
        }
    }
}
