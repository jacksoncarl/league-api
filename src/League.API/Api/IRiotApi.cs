using System.Threading.Tasks;
using League.API.Api.Responses;
using Refit;

namespace League.API.Api
{
    public interface IRiotApi
    {
        [Get("/lol/summoner/v4/summoners/by-name/{summonerName}")]
        Task<GetSummonerByNameResponse> GetSummonerByName(string summonerName);
    }
}