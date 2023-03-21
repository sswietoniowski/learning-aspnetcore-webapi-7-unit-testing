using AutoMapper;
using Hr.Api.Business.Services;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers;

[ApiController]
[Route("api/internal-employees")]
public class InternalEmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;
    private readonly ILogger<InternalEmployeesController> _logger;

    public InternalEmployeesController(IEmployeeService employeeService, IMapper mapper, ILogger<InternalEmployeesController> logger)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]    
    public async Task<ActionResult<IEnumerable<InternalEmployeeDto>>> GetInternalEmployees()
    {
        _logger.LogInformation("Getting all internal employees");

        var internalEmployees = await _employeeService.GetInternalEmployeesAsync();

        // with manual mapping
        var internalEmployeeDtos =
            internalEmployees.Select(e => new InternalEmployeeDto()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary,
                SuggestedBonus = e.SuggestedBonus,
                YearsOfService = e.YearsOfService
            });

        // with AutoMapper
        //var internalEmployeeDtos =
        //    _mapper.Map<IEnumerable<InternalEmployeeDto>>(internalEmployees);

        return Ok(internalEmployeeDtos);
    }

    [HttpGet("{employeeId}")]
    [ActionName(nameof(GetInternalEmployee))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
    public async Task<ActionResult<InternalEmployeeDto>> GetInternalEmployee(Guid? employeeId)
    {
        _logger.LogInformation($"Getting internal employee with id: {employeeId}");

        if (!employeeId.HasValue)
        { 
            return NotFound(); 
        }

        var internalEmployee = await _employeeService.GetInternalEmployeeByIdAsync(employeeId.Value);

        if (internalEmployee == null)
        { 
            return NotFound();
        }             

        return Ok(_mapper.Map<InternalEmployeeDto>(internalEmployee));
    }

    [HttpPost]
    public async Task<ActionResult<InternalEmployeeDto>> CreateInternalEmployee(
        InternalEmployeeForCreationDto internalEmployeeForCreation)
    { 
        _logger.LogInformation($"Creating internal employee with data: \n{internalEmployeeForCreation}");

        // create an internal employee entity with default values filled out
        // and the values inputted via the POST request
        var internalEmployee = await _employeeService.CreateInternalEmployeeAsync(
            internalEmployeeForCreation.FirstName, internalEmployeeForCreation.LastName);

        // persist it
        await _employeeService.CreateInternalEmployeeAsync(internalEmployee);

        // return created employee after mapping to a DTO
        return CreatedAtAction(nameof(GetInternalEmployee),
            new { employeeId = internalEmployee.Id },
            _mapper.Map<InternalEmployeeDto>(internalEmployee));
    }
}