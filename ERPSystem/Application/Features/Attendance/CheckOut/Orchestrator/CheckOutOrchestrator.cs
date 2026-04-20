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

        if (!employeeResult.Data.IsActive)
        {
            return RequestResult<Guid>.Failure("Employee is not active and cannot check out.");
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
        
        double? overtimeHours = 0;
        var checkOutTime = DateTime.UtcNow;
        var overtimeStatus = ERPSystem.Domain.Enums.OvertimeStatus.None;
        
        if (attendanceRecord.CheckInTime.HasValue)
        {
            var workedTime = checkOutTime - attendanceRecord.CheckInTime.Value;
            var standardWorkHours = 8.0;

            if (workedTime.TotalHours > standardWorkHours)
            {
               
                overtimeHours = Math.Round(workedTime.TotalHours - standardWorkHours, 2);
                overtimeStatus = ERPSystem.Domain.Enums.OvertimeStatus.Pending;
            }
        }

        
        var success = await mediator.Send(new CheckOutCommand(attendanceRecord.Id, overtimeHours, overtimeStatus), cancellationToken);

        if (!success)
        {
            return RequestResult<Guid>.Failure("An error occurred during check out.");
        }

        string message = overtimeHours > 0 
            ? $"Checked out successfully. You have {overtimeHours} hours of overtime." 
            : "Checked out successfully.";

        return RequestResult<Guid>.Success(attendanceRecord.Id, message);
    }
}
