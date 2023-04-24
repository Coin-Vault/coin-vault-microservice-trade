using System.Text;
using System.Text.Json;
using TradingService.Dtos;

namespace TradingService.SyncDataServices.Http
{
    public class HttpPortfolioDataClient : IPortfolioDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpPortfolioDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;   
            _configuration = configuration; 
        }

        public async Task SendTradeToPortfolio(TradeReadDto trade)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(trade),
                Encoding.UTF8,
                "application/json");

            var repsonse = await _httpClient.PostAsync($"{_configuration["PortfolioService"]}/api/p/trades/", httpContent);

            if(repsonse.IsSuccessStatusCode)
            {
                Console.WriteLine("Sync POST to portfolio was successfull");
            }    
            else {
                Console.WriteLine("Sync POST to portfolio was NOT successfull");
            }
        }
    }
}