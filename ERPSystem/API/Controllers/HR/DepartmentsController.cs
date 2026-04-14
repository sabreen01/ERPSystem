using ERPSystem.Application.Features.HR.Departments.Commands;
using ERPSystem.Application.Features.HR.Departments.DTOs;
using ERPSystem.Application.Features.HR.Departments.Orchestrator;
using ERPSystem.Application.Features.HR.Departments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.HR;

[ApiController]
[Route("[controller]")]
public class DepartmentsController (IMediator mediator) : BaseController
{
    [HttpPost("Create")]
    public async Task <ActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        return HandleResult(await mediator.Send
            (new CreateDepartmentOrchestrator(dto.Name, dto.Code, dto.ParentDepartmentId)));
    }
    
    [HttpGet("GetAll")]
    public async Task<ActionResult> GetAll()
    {
       return HandleResult(await mediator.Send(new GetAllDepartmentsQuery()));
    
    }

    [HttpGet("GetById/{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        return HandleResult(await mediator.Send(new GetDepartmentByIdQuery(id)));
    }
    
}