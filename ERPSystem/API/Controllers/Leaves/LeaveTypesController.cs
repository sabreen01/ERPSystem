using ERPSystem.Application.Features.Leaves.LeaveTypes.Commands;
using ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;
using ERPSystem.Application.Features.Leaves.LeaveTypes.Orchestrator;
using ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.Leaves;

[ApiController]
[Route("[controller]")]
public class LeaveTypesController(IMediator mediator) : BaseController
{
    [HttpPost("Create")]
    public async Task<ActionResult> Create([FromBody] CreateLeaveTypeDto dto)
    {
        return HandleResult(await mediator.Send(new CreateLeaveTypeOrchestrator(
            dto.Name, dto.Code, dto.DaysPerYear, dto.IsPaid,
            dto.CarryForward, dto.MaxCarryDays, dto.RequiresApproval, dto.MinNoticeDays
        )));
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult> GetAll()
    {
        return HandleResult(await mediator.Send(new GetAllLeaveTypesQuery()));
    }

    [HttpGet("GetById/{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        return HandleResult(await mediator.Send(new GetLeaveTypeByIdQuery(id)));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateLeaveTypeDto dto)
    {
        return HandleResult(await mediator.Send(new UpdateLeaveTypeOrchestrator(id, dto)));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return HandleResult(await mediator.Send(new DeleteLeaveTypeCommand(id)));
    }
}
