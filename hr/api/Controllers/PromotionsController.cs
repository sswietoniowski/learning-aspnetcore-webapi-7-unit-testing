using Hr.Api.Business.Services;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers;

[ApiController]
[Route("api/promotions")]
public class PromotionsController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IPromotionService _promotionService;
    private readonly ILogger<PromotionsController>? _logger;

    public PromotionsController(IEmployeeService employeeService, IPromotionService promotionService, ILogger<PromotionsController>? logger)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        _promotionService = promotionService ?? throw new ArgumentNullException(nameof(promotionService));
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePromotion(PromotionForCreationDto promotionForCreation)
    {
        _logger?.LogInformation($"Creating promotion with data: \n{promotionForCreation}");

        var internalEmployeeToPromote = await _employeeService.GetInternalEmployeeByIdAsync(promotionForCreation.EmployeeId);

        if (internalEmployeeToPromote == null)
        {
            return BadRequest();
        }

        if (await _promotionService.PromoteInternalEmployeeAsync(internalEmployeeToPromote))
        {
            _logger?.LogInformation($"Employee with id: {internalEmployeeToPromote.Id} was promoted to job level: {internalEmployeeToPromote.JobLevel}.");

            return Ok
            (
                new PromotionDto()
                {
                    EmployeeId = internalEmployeeToPromote.Id,
                    JobLevel = internalEmployeeToPromote.JobLevel
                }
            );
        }
        else
        {
            _logger?.LogInformation($"Employee with id: {internalEmployeeToPromote.Id} was not eligible for promotion.");

            return BadRequest("Employee not eligible for promotion.");
        }
    }
}