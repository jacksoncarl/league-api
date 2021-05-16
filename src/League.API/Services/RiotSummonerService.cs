using System;
using System.Net;
using System.Threading.Tasks;
using League.API.Api;
using League.API.Api.Responses;
using League.API.Models;
using OneOf;
using Refit;

namespace League.API.Services
{
    public interface IRiotSummonerService
    {
        Task<OneOf<GetSummonerByNameResult, GetSummonerByNameNotFound, GetSummonerByNameError>> GetBySummonerName(GetSummonerByNameRequest request);
    }

    public class RiotSummonerService : IRiotSummonerService
    {
        private readonly IRiotApi _riotApi;

        public RiotSummonerService(IRiotApi riotApi)
        {
            _riotApi = riotApi ?? throw new ArgumentNullException(nameof(riotApi));
        }

        public async Task<OneOf<GetSummonerByNameResult, GetSummonerByNameNotFound, GetSummonerByNameError>> GetBySummonerName(GetSummonerByNameRequest request)
        {
            try
            {
                GetSummonerByNameResponse response = await _riotApi.GetSummonerByName(request.SummonerName);

                return new GetSummonerByNameResult
                {
                    AccountId = response.AccountId,
                    ProfileIconId = response.ProfileIconId,
                    RevisionDate = response.RevisionDate,
                    Name = response.Name,
                    Id = response.Id,
                    PUuid = response.Puuid,
                    SummonerLevel = response.SummonerLevel
                };
            }
            catch (ApiException exception)
            {
                return exception.StatusCode switch
                {
                    HttpStatusCode.NotFound => new GetSummonerByNameNotFound(),
                    _ => new GetSummonerByNameError(new[] { Errors.UnexpectedError }),
                };
            }
        }
    }
}