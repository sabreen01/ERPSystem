using ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Enums;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Orchestrator;

public record CancelLeaveRequestOrchestrator(Guid LeaveRequestId, Guid RequestedBy) : IRequest<RequestResult<string>>;

public class CancelLeaveRequestOrchestratorHandler(IMediator mediator)
    : IRequestHandler<CancelLeaveRequestOrchestrator, RequestResult<string>>
{
    public async Task<RequestResult<string>> Handle(CancelLeaveRequestOrchestrator request, CancellationToken cancellationToken)
    {
       
        var leaveRequest = await mediator.Send(new GetLeaveRequestByIdQuery(request.LeaveRequestId), cancellationToken);
        
        if (leaveRequest == null)
            return RequestResult<string>.Failure("Leave request not found.");

        if (leaveRequest.StartDate.Date <= DateTime.UtcNow.Date)
            return RequestResult<string>.Failure("Cannot cancel a leave request that has already started or is in the past.");

        
        if (leaveRequest.Status == LeaveRequestStatus.Rejected || leaveRequest.Status == LeaveRequestStatus.Cancelled)
            return RequestResult<string>.Failure($"Leave request is already {leaveRequest.Status}.");

        var wasApproved = leaveRequest.Status == LeaveRequestStatus.Approved;

        var updateResult = await mediator.Send(
            new UpdateLeaveRequestStatusCommand(request.LeaveRequestId, LeaveRequestStatus.Cancelled, "Cancelled by user", request.RequestedBy),
            cancellationToken);

        if (!updateResult)
            return RequestResult<string>.Failure("Failed to cancel leave request.");

       
        if (wasApproved)
        {
           
            int fiscalYear = leaveRequest.StartDate.Year;
            await mediator.Send(
                new RefundLeaveBalanceUsedDaysCommand(leaveRequest.EmployeeId, leaveRequest.LeaveTypeId, fiscalYear, leaveRequest.DaysCount),
                cancellationToken);

           
            var requestedDates = leaveRequest.LeaveRequestDays.Select(d => d.Date).ToList();
            await mediator.Send(
                new DeleteLeaveAttendanceRecordsCommand(leaveRequest.EmployeeId, requestedDates),
                cancellationToken);
        }

        return RequestResult<string>.Success("Leave request cancelled successfully.");
    }
}
