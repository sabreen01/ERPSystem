using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;

public record UpdateLeaveRequestStatusCommand(
    Guid LeaveRequestId,
    LeaveRequestStatus Status,
    string? RejectionReason,
    Guid ReviewedBy
) : IRequest<bool>;

public class UpdateLeaveRequestStatusCommandHandler(IRepository<LeaveRequest> repository)
    : IRequestHandler<UpdateLeaveRequestStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateLeaveRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await repository.GetById(request.LeaveRequestId);
        if (leaveRequest == null) return false;

        leaveRequest.Status = request.Status;
        leaveRequest.ApprovedBy = request.ReviewedBy;
        leaveRequest.ApprovedAt = DateTime.UtcNow;
        leaveRequest.RejectionReason = request.RejectionReason;

        repository.Update(leaveRequest);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
