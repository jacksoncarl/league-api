using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using League.API.Models;
using League.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace League.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummonerController : ControllerBase
    {
        private readonly ILogger<SummonerController> _logger;
        private readonly IRiotSummonerService _riotSummonerService;

        public SummonerController(ILogger<SummonerController> logger, IRiotSummonerService riotSummonerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _riotSummonerService = riotSummonerService ?? throw new ArgumentNullException(nameof(riotSummonerService));
        }

        [HttpGet("{summonerName}")]
        [ProducesResponseType(typeof(GetSummonerByNameResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetByName([Required] string summonerName)
        {
            var request = new GetSummonerByNameRequest() {SummonerName = summonerName};
            var result = await _riotSummonerService.GetBySummonerName(request);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}