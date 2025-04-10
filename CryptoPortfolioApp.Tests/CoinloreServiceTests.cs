using CryptoPortfolioApp.Components.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace CryptoPortfolioApp.Tests
{
    [TestFixture]
    public class CoinloreServiceTests
    {
        [Test]
        public async Task GetCoinsAsync_ShouldReturnCoins_WhenResponseIsValid()
        {
            // Arrange
            var expectedJson = @"{
                ""data"": [
                    {
                        ""id"": ""1"",
                        ""symbol"": ""BTC"",
                        ""name"": ""Bitcoin"",
                        ""price_usd"": ""25000""
                    }
                ]
            }";

            // Create a mock of HttpMessageHandler.
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var loggerMock = new Mock<ILogger<CoinloreService>>();

            var coinloreService = new CoinloreService(httpClient, loggerMock.Object);

            // Act
            var coins = await coinloreService.GetCoinsAsync();

            // Assert
            var coin = coins[0];
            Assert.That(coin.Symbol, Is.EqualTo("BTC"));
            Assert.That(coin.Name, Is.EqualTo("Bitcoin"));
            Assert.That(coin.PriceUsd, Is.EqualTo("25000"));
        }
    }
}
