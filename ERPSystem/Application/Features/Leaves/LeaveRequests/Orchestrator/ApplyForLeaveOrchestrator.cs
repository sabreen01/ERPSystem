using ERPSystem.Application.Features.HR.Employees.Queries;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Queries;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;
using ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;
using ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Application.Interfaces;
using ERPSystem.Domain.Enums;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Orchestrator;

public record ApplyForLeaveOrchestrator(ApplyForLeaveDto Data) 
    : IRequest<RequestResult<Guid>>;

public class ApplyForLeaveOrchestratorHandler(IMediator mediator, ILeaveCalculatorService leaveCalculatorService)
    : IRequestHandler<ApplyForLeaveOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(ApplyForLeaveOrchestrator request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        if (dto.SelectedDates == null || !dto.SelectedDates.Any())
            return RequestResult<Guid>.Failure("No dates selected.");

        var utcDates = dto.SelectedDates.Select(d => DateTime.SpecifyKind(d, DateTimeKind.Utc).Date)
            .Distinct()
            .OrderBy(d => d)
            .ToList();
        
        var startDateUtc = utcDates.First();
        var endDateUtc = utcDates.Last();

        if (startDateUtc < DateTime.UtcNow.Date)
            return RequestResult<Guid>.Failure("Start date cannot be in the past.");

        var validDates = await leaveCalculatorService.FilterValidLeaveDaysAsync(utcDates, cancellationToken);
        var daysCount = validDates.Count;
        
        if (daysCount == 0)
            return RequestResult<Guid>.Failure("The selected dates contain only weekends or public holidays, resulting in 0 leave days.");

        var employeeResult = await mediator.Send(new GetEmployeeByIdQuery(dto.EmployeeId), cancellationToken);
        if (!employeeResult.IsSuccess)
            return RequestResult<Guid>.Failure("Employee not found.");

        if (!employeeResult.Data.IsActive)
            return RequestResult<Guid>.Failure("Employee is not active.");
       
        var leaveTypeResult = await mediator.Send(new GetLeaveTypeByIdQuery(dto.LeaveTypeId), cancellationToken);
        if (!leaveTypeResult.IsSuccess)
            return RequestResult<Guid>.Failure("Leave type not found.");

        if (!leaveTypeResult.Data.IsActive)
            return RequestResult<Guid>.Failure("Leave type is not active.");

       
        var noticeDays = (startDateUtc - DateTime.UtcNow.Date).Days;
        if (noticeDays < leaveTypeResult.Data.MinNoticeDays)
        {
            return RequestResult<Guid>.Failure($"This leave type requires at least {leaveTypeResult.Data.MinNoticeDays} days of advance notice.");
        }
        
        var isOverlapping = await mediator.Send(
            new CheckOverlappingLeaveQuery(dto.EmployeeId, validDates), cancellationToken);
            
        if (isOverlapping)
            return RequestResult<Guid>.Failure("The requested leave overlaps with an existing leave request.");

        var fiscalYear = startDateUtc.Year;
        var balancesResult = await mediator.Send(new GetEmployeeLeaveBalancesQuery(dto.EmployeeId, fiscalYear), cancellationToken);
        
        if (!balancesResult.IsSuccess)
            return RequestResult<Guid>.Failure("Could not fetch leave balances.");

        var specificBalance = balancesResult.Data.FirstOrDefault(b => b.LeaveTypeId == dto.LeaveTypeId);
        if (specificBalance == null)
            return RequestResult<Guid>.Failure($"No leave balance found for this employee for {fiscalYear}.");

        if (specificBalance.RemainingBalance < daysCount)
            return RequestResult<Guid>.Failure($"Insufficient leave balance. Requested: {daysCount}, Available: {specificBalance.RemainingBalance}.");
       
        
        var requiresApproval = leaveTypeResult.Data.RequiresApproval;
        var initialStatus = requiresApproval ? LeaveRequestStatus.Pending : LeaveRequestStatus.Approved;

        var command = new ApplyForLeaveCommand(
            dto.EmployeeId,
            dto.LeaveTypeId,
            startDateUtc,
            endDateUtc,
            validDates,
            daysCount,
            dto.Reason,
            initialStatus
        );

        var leaveRequestId = await mediator.Send(command, cancellationToken);

        if (!requiresApproval)
        {
           
            await mediator.Send(
                new UpdateLeaveBalanceUsedDaysCommand(
                    dto.EmployeeId,
                    dto.LeaveTypeId,
                    fiscalYear,
                    daysCount), cancellationToken);

            
            await mediator.Send(new GenerateLeaveAttendanceRecordsCommand(
                dto.EmployeeId, validDates), cancellationToken);

            return RequestResult<Guid>.Success(leaveRequestId, "Leave request submitted and automatically approved.");
        }

        return RequestResult<Guid>.Success(leaveRequestId, "Leave request submitted successfully and is pending approval.");
    }
}
