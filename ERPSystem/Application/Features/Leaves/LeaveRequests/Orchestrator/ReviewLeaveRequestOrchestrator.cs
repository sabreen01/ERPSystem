using ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;
using ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;
using ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Enums;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Orchestrator;

public record ReviewLeaveRequestOrchestrator(ReviewLeaveRequestDto Data) 
    : IRequest<RequestResult<string>>;

public class ReviewLeaveRequestOrchestratorHandler(IMediator mediator)
    : IRequestHandler<ReviewLeaveRequestOrchestrator, RequestResult<string>>
{
    public async Task<RequestResult<string>> Handle(ReviewLeaveRequestOrchestrator request, CancellationToken cancellationToken)
    {
        var dto = request.Data;

        var leaveRequest = await mediator.Send(new GetLeaveRequestByIdQuery(dto.LeaveRequestId), cancellationToken);
        if (leaveRequest == null)
            return RequestResult<string>.Failure("Leave request not found.");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            return RequestResult<string>.Failure($"Cannot review a leave request that is already {leaveRequest.Status}.");

       
        if (dto.Status == LeaveRequestStatus.Approved)
        {
            
            var updateResult = await mediator.Send(
                new UpdateLeaveRequestStatusCommand(dto.LeaveRequestId, LeaveRequestStatus.Approved, null, dto.ReviewedBy), 
                cancellationToken);
                
            if (!updateResult)
                return RequestResult<string>.Failure("Failed to update leave request status.");

           
            int fiscalYear = leaveRequest.StartDate.Year;
            var balanceResult = await mediator.Send(
                new UpdateLeaveBalanceUsedDaysCommand(leaveRequest.EmployeeId, leaveRequest.LeaveTypeId, fiscalYear, leaveRequest.DaysCount), 
                cancellationToken);

            if (!balanceResult)
                return RequestResult<string>.Failure("Warning: Leave request approved, but failed to update employee's leave balance.");

            
            var requestedDates = leaveRequest.LeaveRequestDays.Select(d => d.Date).ToList();
            await mediator.Send(
                new GenerateLeaveAttendanceRecordsCommand(
                    leaveRequest.EmployeeId, requestedDates), cancellationToken);

            return RequestResult<string>.Success("Leave request approved successfully.");
        }
        else if (dto.Status == LeaveRequestStatus.Rejected)
        {
            if (string.IsNullOrWhiteSpace(dto.RejectionReason))
                return RequestResult<string>.Failure("A rejection reason is required.");

            var updateResult = await mediator.Send(
                new UpdateLeaveRequestStatusCommand(dto.LeaveRequestId, LeaveRequestStatus.Rejected, dto.RejectionReason, dto.ReviewedBy), 
                cancellationToken);

            if (!updateResult)
                return RequestResult<string>.Failure("Failed to update leave request status.");

            return RequestResult<string>.Success("Leave request rejected successfully.");
        }

        return RequestResult<string>.Failure("Invalid review status provided.");
    }
}
