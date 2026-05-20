using ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Orchestrator;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.Leaves;

[ApiController]
[Route("[controller]")]
public class LeaveRequestsController(IMediator mediator) : BaseController
{
    [HttpPost("Apply")]
    public async Task<ActionResult> Apply([FromBody] ApplyForLeaveDto dto)
    {
        return HandleResult(await mediator.Send(new ApplyForLeaveOrchestrator(dto)));
    }

    [HttpPost("Review")]
    public async Task<ActionResult> Review([FromBody] ReviewLeaveRequestDto dto)
    {
        return HandleResult(await mediator.Send(new ReviewLeaveRequestOrchestrator(dto)));
    }

    [HttpGet("Pending")]
    public async Task<ActionResult> GetPending()
    {
        return HandleResult(await mediator.Send(new GetPendingLeaveRequestsQuery()));
    }

    [HttpPost("Cancel")]
    public async Task<ActionResult> Cancel([FromQuery] Guid leaveRequestId, [FromQuery] Guid requestedBy)
    {
        return HandleResult(await mediator.Send(new CancelLeaveRequestOrchestrator(leaveRequestId, requestedBy)));
    }
}
