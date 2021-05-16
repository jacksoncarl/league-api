using System.Collections.Generic;

namespace League.API.Models
{
    public record GetSummonerByNameResult
    {
        public string AccountId { get; init; }

        public int ProfileIconId { get; init; }

        public long RevisionDate { get; init; }

        public string Name { get; init; }

        public string Id { get; init; }

        public string PUuid { get; init; }

        public long SummonerLevel { get; init; }
    }

    public struct GetSummonerByNameNotFound
    {
    }

    public struct GetSummonerByNameError
    {
        public IReadOnlyList<string> ErrorMessages { get; init; }

        public GetSummonerByNameError(IReadOnlyList<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}