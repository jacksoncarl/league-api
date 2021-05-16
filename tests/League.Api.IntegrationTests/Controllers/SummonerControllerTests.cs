using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using League.API;
using League.API.Api;
using League.Api.IntegrationTests.Fakes;
using League.Api.IntegrationTests.Models;
using League.API.Models;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
                PUuid = "6WWJyoBhf9xdkCAWnypkUcy08U7lQORIPWqymtUf-7v_NWqqCfry7P5Eotgh-gJBDkejlX4eP_MLLg",
                SummonerLevel = 107
            };

            var response = await _client.GetFromJsonAsync<ExpectedGetSummonerByNameResult>("kaelsin");

            Assert.NotNull(response);

            //Could improve this using an equality comparer?
            Assert.Equal(summoner.AccountId, response.AccountId);
            Assert.Equal(summoner.ProfileIconId, response.ProfileIconId);
            Assert.Equal(summoner.RevisionDate, response.RevisionDate);
            Assert.Equal(summoner.Name, response.Name);
            Assert.Equal(summoner.Id, response.Id);
            Assert.Equal(summoner.PUuid, response.PUuid);
            Assert.Equal(summoner.SummonerLevel, response.SummonerLevel);
        }

        [Theory]
        [InlineData("", "summonerName", "Please provide a valid summoner name.")]
        [InlineData("%20", "summonerName", "Please provide a valid summoner name.")]
        [InlineData("12", "summonerName", "summonerName must be at least 3 characters long and no more than 16 characters long.")]
        [InlineData("58044623721529723", "summonerName", "summonerName must be at least 3 characters long and no more than 16 characters long.")]
        public async Task Get_WithoutSummonerName_ReturnsBadRequest(string requestUri, string expectedPropertyName, string expectedErrorMessage)
        {
            var response = await _client.GetAsync(requestUri);

            var errorDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(expectedErrorMessage, errorDetails.Message);
            Assert.Equal(expectedPropertyName, errorDetails.ParameterName);
        }

        [Fact]
        public async Task Get_UnexpectedException_ReturnsInternalServerError()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IRiotApi>(new FakeRiotApi() { ShouldThrow = true });
                });
            }).CreateClient();

            var response = await client.GetAsync("kaelsin");

            var errorDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("An unexpected error occurred.", errorDetails.Message);
        }
    }
}