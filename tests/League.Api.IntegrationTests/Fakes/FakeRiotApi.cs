using System.Net.Http;
using System.Threading.Tasks;
using League.API.Api;
using League.API.Api.Responses;
using Refit;

namespace League.Api.IntegrationTests.Fakes
{
    public class FakeRiotApi : IRiotApi
    {
        public bool ShouldThrow { get; set; }

        public async Task<GetSummonerByNameResponse> GetSummonerByName(string summonerName)
        {
            if (ShouldThrow)
            {
                var apiException = await ApiException.Create(new HttpRequestMessage(), HttpMethod.Get, new HttpResponseMessage(), null);;
                throw apiException;
            }
            var summoner = new GetSummonerByNameResponse()
            {
                AccountId = "9OnF1YN-vTD7-vzFmVafiDk31yudiaRF1V9RaQR-ygE",
                ProfileIconId = 4568,
                RevisionDate = 1620858743000,
                Name = "Kaelsin",
                Id = "M_uI3RI7JlQlY2kZXz4LuvmR7ilm6OlvPKcJHfycM7ih",
                Puuid = "6WWJyoBhf9xdkCAWnypkUcy08U7lQORIPWqymtUf-7v_NWqqCfry7P5Eotgh-gJBDkejlX4eP_MLLg",
                SummonerLevel = 107
            };

            return summoner;
        }
    }
}