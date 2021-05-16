namespace League.API
{
    public static class Errors
    {
        public const string SummonerNameLength = "summonerName must be at least {2} characters long and no more than {1} characters long.";
        public const string SummonerNotFound = "This summoner could not be found.";
        public const string SummonerNameNotProvided = "Please provide a valid summoner name.";
        public const string UnexpectedError = "An unexpected error occurred.";
    }
}
