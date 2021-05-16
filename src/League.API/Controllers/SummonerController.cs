using System;
using System.Net;
using System.Threading.Tasks;
using League.API.Models;
using League.API.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public IActionResult Get()
        {
            return BadRequest(ErrorResponse.GenerateErrorResponse("summonerName", Errors.SummonerNameNotProvided));
        }

        [HttpGet("{summonerName}")]
        [ProducesResponseType(typeof(GetSummonerByNameResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetByName([FromRoute] GetSummonerByNameRequest request)
        {
            var result = await _riotSummonerService.GetBySummonerName(request);

            return result.Match<IActionResult>(
                Ok,
                notFound => NotFound(ErrorResponse.GenerateErrorResponse("summonerName", Errors.SummonerNotFound)),
                error => StatusCode(StatusCodes.Status500InternalServerError, ErrorResponse.GenerateErrorResponse(null, Errors.UnexpectedError))
            );
        }
    }
}