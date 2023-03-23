using AutoMapper;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IMapper _mapper;

    public StatisticsController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StatisticsDto> GetStatistics()
    {
        var httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();

        return Ok(_mapper.Map<StatisticsDto>(httpConnectionFeature));
    }
}