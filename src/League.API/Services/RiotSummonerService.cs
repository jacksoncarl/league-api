using System;
using System.Threading.Tasks;
using League.API.Api;
using League.API.Models;

namespace League.API.Services
{
    public interface IRiotSummonerService
    {
        Task<GetSummonerByNameResult> GetBySummonerName(GetSummonerByNameRequest request);
    }

    public class RiotSummonerService : IRiotSummonerService
    {
        private readonly IRiotApi _riotApi;

        public RiotSummonerService(IRiotApi riotApi)
        {
            _riotApi = riotApi ?? throw new ArgumentNullException(nameof(riotApi));
        }

        public async Task<GetSummonerByNameResult> GetBySummonerName(GetSummonerByNameRequest request)
        {
            var response = await _riotApi.GetSummonerByName(request.SummonerName);
            return new GetSummonerByNameResult
            {
                AccountId = response.AccountId,
                ProfileIconId = response.ProfileIconId,
                RevisionDate = response.RevisionDate,
                Name = response.Name,
                Id = response.Id,
                Puuid = response.Puuid,
                SummonerLevel = response.SummonerLevel
            };
        }
    }
}