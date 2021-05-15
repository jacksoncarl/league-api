namespace League.API.Api.Responses
{
    public record GetSummonerByNameResponse
    {
        public string AccountId { get; init; }

        public int ProfileIconId { get; init; }

        public long RevisionDate { get; init; }

        public string Name { get; init; }

        public string Id { get; init; }

        public string Puuid { get; init; }

        public long SummonerLevel { get; init; }
    }
}