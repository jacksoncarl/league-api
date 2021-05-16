using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace League.API.Models
{
    public class GetSummonerByNameRequest
    {
        [FromRoute(Name = "summonerName")]
        [Required(ErrorMessage = Errors.SummonerNameNotProvided)]
        [StringLength(16, ErrorMessage = Errors.SummonerNameLength, MinimumLength = 3)]
        public string SummonerName { get; init; }
    }
}