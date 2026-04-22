using ERPSystem.Application.Features.Attendance.CheckIn.Orchestrator;
using ERPSystem.Application.Features.Attendance.CheckOut.Orchestrator;
using ERPSystem.Application.Features.Attendance.Overtime.Commands;
using ERPSystem.Application.Features.Attendance.Overtime.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.Attendance;

[ApiController]
[Route("[controller]")]
public class AttendanceController(IMediator mediator) : BaseController
{
    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] Guid employeeId)
    {
        return HandleResult(await mediator.Send(new CheckInOrchestrator(employeeId)));
    }

    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] Guid employeeId)
    {
        return HandleResult(await mediator.Send(new CheckOutOrchestrator(employeeId)));
    }

    [HttpGet("overtime/pending")]
    public async Task<IActionResult> GetPendingOvertimes()
    {
        return HandleResult(await mediator.Send(new GetPendingOvertimesQuery()));
    }

    [HttpPost("overtime/review")]
    public async Task<IActionResult> ReviewOvertime([FromBody] ReviewOvertimeCommand command)
    {
        return HandleResult(await mediator.Send(command));
    }
}
