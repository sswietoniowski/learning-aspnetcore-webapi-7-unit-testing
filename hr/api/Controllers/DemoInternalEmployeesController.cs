using AutoMapper;
using Hr.Api.Business.Services;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers;

[Microsoft.AspNetCore.Components.Route("api/demo-internal-employees")]
public class DemoInternalEmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public DemoInternalEmployeesController(IEmployeeService employeeService,
        IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<InternalEmployeeDto>> CreateInternalEmployee(
        InternalEmployeeForCreationDto internalEmployeeForCreation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // create an internal employee entity with default values filled out
        // and the values inputted via the POST request
        var internalEmployee =
            await _employeeService.CreateInternalEmployeeAsync(
                internalEmployeeForCreation.FirstName, internalEmployeeForCreation.LastName);

        // persist it
        await _employeeService.CreateInternalEmployeeAsync(internalEmployee);

        // return created employee after mapping to a DTO
        // ReSharper disable once Mvc.ActionNotResolved
        return CreatedAtAction("GetInternalEmployee",
            _mapper.Map<InternalEmployeeDto>(internalEmployee),
            new { employeeId = internalEmployee.Id });
    }


    [HttpGet]
    [Authorize]
    public IActionResult GetProtectedInternalEmployees()
    {
        // depending on the role, redirect to another action
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction(
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
                "GetInternalEmployees", "ProtectedInternalEmployees");
        }

        return RedirectToAction("GetInternalEmployees", "InternalEmployees");
    }

}