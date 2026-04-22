using ERPSystem.Application.Features.HR.Employees.Commands;
using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Application.Features.HR.Employees.Orchestrator;
using ERPSystem.Application.Features.HR.Employees.Queries;
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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllEmployeesQuery query)
    {
        return HandleResult(await mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return HandleResult(await mediator.Send(new GetEmployeeByIdQuery(id)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto dto)
    {
        return HandleResult(await mediator.Send(new UpdateEmployeeOrchestrator(id, dto)));
    }

    [HttpPost("{id}/photo")]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile photo)
    {
        return HandleResult(await mediator.Send(new UploadEmployeePhotoCommand(id, photo)));
    }

}