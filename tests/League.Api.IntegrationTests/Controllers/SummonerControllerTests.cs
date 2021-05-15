using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using League.API;
using League.Api.IntegrationTests.Models;
using League.API.Models;
using Xunit;

namespace League.Api.IntegrationTests.Controllers
{
    public class SummonerControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public SummonerControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost/api/summoner/");

            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsExpectedSummonerProfile()
        {
            var summoner = new GetSummonerByNameResult
            {
                AccountId = "9OnF1YN-vTD7-vzFmVafiDk31yudiaRF1V9RaQR-ygE",
                ProfileIconId = 4568,
                RevisionDate = 1620858743000,
                Name = "Kaelsin",
                Id = "M_uI3RI7JlQlY2kZXz4LuvmR7ilm6OlvPKcJHfycM7ih",
                Puuid = "6WWJyoBhf9xdkCAWnypkUcy08U7lQORIPWqymtUf-7v_NWqqCfry7P5Eotgh-gJBDkejlX4eP_MLLg",
                SummonerLevel = 107
            };

            var response = await _client.GetFromJsonAsync<ExpectedGetSummonerByNameResult>("kaelsin");

            Assert.NotNull(response);

            //Could improve this using an equality comparer?
            Assert.Equal(response.AccountId, summoner.AccountId);
            Assert.Equal(response.ProfileIconId, summoner.ProfileIconId);
            Assert.Equal(response.RevisionDate, summoner.RevisionDate);
            Assert.Equal(response.Name, summoner.Name);
            Assert.Equal(response.Id, summoner.Id);
            Assert.Equal(response.Puuid, summoner.Puuid);
            Assert.Equal(response.SummonerLevel, summoner.SummonerLevel);
        }
    }
}