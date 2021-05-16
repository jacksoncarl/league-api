namespace League.Api.IntegrationTests.Models
{
    public record ExpectedGetSummonerByNameResult
    {
        public string AccountId { get; init; }

        public int ProfileIconId { get; init; }

        public long RevisionDate { get; init; }

        public string Name { get; init; }

        public string Id { get; init; }

        public string PUuid { get; init; }

        public long SummonerLevel { get; init; }
    }
}