using CryptoPortfolioApp.Components.Interfaces;
using CryptoPortfolioApp.Components.Models;
using CryptoPortfolioApp.Components.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CryptoPortfolioApp.Tests
{
    [TestFixture]
    public class PortfolioCalculatorServiceTests
    {
        private Mock<ICoinloreService> _coinloreServiceMock;
        private Mock<ILogger<PortfolioCalculatorService>> _loggerMock;
        private PortfolioCalculatorService _service;

        [SetUp]
        public void Setup()
        {
            _coinloreServiceMock = new Mock<ICoinloreService>();
            _loggerMock = new Mock<ILogger<PortfolioCalculatorService>>();

            _service = new PortfolioCalculatorService(_coinloreServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CalculateAsync_ShouldReturnCorrectPortfolioCalculationResult()
        {
            // Arrange
            var entries = new List<PortfolioEntry>
            {
                new PortfolioEntry { Amount = 2, Coin = "BTC", BuyPrice = 20000 },
                new PortfolioEntry { Amount = 5, Coin = "ETH", BuyPrice = 1000 }
            };

            var coinData = new List<Coin>
            {
                new Coin { Symbol = "BTC", PriceUsd = "25000", Name = "Bitcoin", Id = "1" },
                new Coin { Symbol = "ETH", PriceUsd = "1500", Name = "Ethereum", Id = "2" }
            };

            _coinloreServiceMock.Setup(x => x.GetCoinsAsync()).ReturnsAsync(coinData);

            // Act
            var result = await _service.CalculateAsync(entries);

            // Assert - Calculate expected results:
            // BTC: initial value = 2 * 20000 = 40000, current value = 2 * 25000 = 50000, percentage change = ((25000 - 20000) / 20000) * 100 = 25%
            // ETH: initial value = 5 * 1000 = 5000, current value = 5 * 1500 = 7500, percentage change = ((1500 - 1000) / 1000) * 100 = 50%
            // Overall initial total = 40000 + 5000 = 45000
            // Overall current total = 50000 + 7500 = 57500
            // Overall change = 57500 - 45000 = 12500, overall change percentage = (12500 / 45000) * 100 = 27.78%

            Assert.That(result.InitialTotal, Is.EqualTo(45000.0d));
            Assert.That(result.CurrentTotal, Is.EqualTo(57500.0d));
            Assert.That(result.OverallChange, Is.EqualTo(12500.0d));
            Assert.That(result.OverallChangePercentage, Is.EqualTo(27.78).Within(0.01));

            // Assert individual coin calculation for BTC
            var btcResult = result.Coins.Find(x => string.Equals(x.Coin, "BTC", StringComparison.OrdinalIgnoreCase));
            Assert.That(btcResult.InitialValue, Is.EqualTo(40000.0d));
            Assert.That(btcResult.CurrentValue, Is.EqualTo(50000.0d));
            Assert.That(btcResult.PercentageChange, Is.EqualTo(25).Within(0.01));

            // Assert individual coin calculation for ETH
            var ethResult = result.Coins.Find(x => string.Equals(x.Coin, "ETH", StringComparison.OrdinalIgnoreCase));
            Assert.That(ethResult.InitialValue, Is.EqualTo(5000.0d));
            Assert.That(ethResult.CurrentPrice, Is.EqualTo(1500.0d));
            Assert.That(ethResult.PercentageChange, Is.EqualTo(50).Within(0.01));
        }

        [Test]
        public async Task CalculateAsync_ShouldLogWarning_WhenCoinDataNotFound()
        {
            // Arrange
            var entries = new List<PortfolioEntry>
            {
                new PortfolioEntry { Amount = 1, Coin = "XRP", BuyPrice = 0.5 }
            };

            var coinData = new List<Coin>
            {
                new Coin { Symbol = "BTC", PriceUsd = "25000", Name = "Bitcoin", Id = "1" },
                new Coin { Symbol = "ETH", PriceUsd = "1500", Name = "Ethereum", Id = "2" }
            };

            _coinloreServiceMock.Setup(x => x.GetCoinsAsync()).ReturnsAsync(coinData);

            // Act
            var result = await _service.CalculateAsync(entries);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No data found for coin: XRP")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                    ),
                Times.Once());
        }
    }
}
