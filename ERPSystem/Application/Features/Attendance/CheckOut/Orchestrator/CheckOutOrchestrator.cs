using ERPSystem.Application.Features.Attendance.CheckOut.Commands;
using ERPSystem.Application.Features.Attendance.CheckOut.Queries;
using ERPSystem.Application.Features.HR.Employees.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.Attendance.CheckOut.Orchestrator;

public record CheckOutOrchestrator(Guid EmployeeId) : IRequest<RequestResult<Guid>>;

public class CheckOutOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<CheckOutOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CheckOutOrchestrator request, CancellationToken cancellationToken)
    {
       
        var employeeResult = await mediator.Send(new GetEmployeeByIdQuery(request.EmployeeId), cancellationToken);
        if (!employeeResult.IsSuccess)
        {
            return RequestResult<Guid>.Failure("Employee not found.");
        }

       
        var attendanceRecord = await mediator.Send(new GetTodayAttendanceQuery(request.EmployeeId), cancellationToken);
        
        if (attendanceRecord == null)
        {
            return RequestResult<Guid>.Failure("Employee has not checked in today. Please check in first.");
        }

        if (attendanceRecord.CheckOutTime.HasValue)
        {
            return RequestResult<Guid>.Failure("Employee has already checked out today.");
        }

      
        var success = await mediator.Send(new CheckOutCommand(attendanceRecord.Id), cancellationToken);

        if (!success)
        {
            return RequestResult<Guid>.Failure("An error occurred during check out.");
        }

        return RequestResult<Guid>.Success(attendanceRecord.Id, "Checked out successfully.");
    }
}
