using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Application.Features.HR.Employees.Orchestrator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.HR;

[ApiController]
[Route("[controller]")]
public class EmployeesController(IMediator mediator):BaseController
{

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        return HandleResult(await mediator.Send(new CreateEmployeeOrchestrator(dto)));
    }

}