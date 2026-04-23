using ERPSystem.Application.Features.Attendance.CheckIn.Commands;
using ERPSystem.Application.Features.Attendance.CheckIn.Queries;
using ERPSystem.Application.Features.HR.Employees.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Enums;
using MediatR;

namespace ERPSystem.Application.Features.Attendance.CheckIn.Orchestrator;

public record CheckInOrchestrator(Guid EmployeeId) : IRequest<RequestResult<Guid>>;

public class CheckInOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<CheckInOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CheckInOrchestrator request, CancellationToken cancellationToken)
    {
       
        var employeeResult = await mediator.Send(new GetEmployeeByIdQuery(request.EmployeeId), cancellationToken);
        if (!employeeResult.IsSuccess)
        {
            return RequestResult<Guid>.Failure("Employee not found.");
        }

        if (!employeeResult.Data.IsActive)
        {
            return RequestResult<Guid>.Failure("Employee is not active and cannot check in.");
        }

       
        var hasCheckedIn = await mediator.Send(new CheckEmployeeAttendanceTodayQuery(request.EmployeeId), cancellationToken);
        if (hasCheckedIn)
        {
            return RequestResult<Guid>.Failure("Employee has already checked in today.");
        }

       
        var localTime = DateTime.Now.TimeOfDay; 
        var shiftStartTime = new TimeSpan(9, 0, 0); 
        var gracePeriod = new TimeSpan(0, 15, 0);

        var status = ERPSystem.Domain.Enums.AttendanceStatus.Present;
        
        if (localTime > shiftStartTime.Add(gracePeriod))
        {
            status = AttendanceStatus.Late;
        }

       
        var attendanceId = await mediator.Send(new CheckInCommand(request.EmployeeId, status), cancellationToken);

        string message = status == AttendanceStatus.Late 
            ? "Checked in successfully, but marked as Late." 
            : "Checked in successfully.";

        return RequestResult<Guid>.Success(attendanceId, message);
    }
}
