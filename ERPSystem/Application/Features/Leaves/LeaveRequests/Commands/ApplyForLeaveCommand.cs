using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;

public record ApplyForLeaveCommand(
    Guid EmployeeId,
    Guid LeaveTypeId,
    DateTime StartDate,
    DateTime EndDate,
    List<DateTime> SelectedDates,
    int DaysCount,
    string? Reason,
    LeaveRequestStatus Status = LeaveRequestStatus.Pending
) : IRequest<Guid>;

public class ApplyForLeaveCommandHandler(IRepository<LeaveRequest> repository)
    : IRequestHandler<ApplyForLeaveCommand, Guid>
{
    public async Task<Guid> Handle(ApplyForLeaveCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = new LeaveRequest
        {
            EmployeeId = request.EmployeeId,
            LeaveTypeId = request.LeaveTypeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DaysCount = request.DaysCount,
            Reason = request.Reason,
            Status = request.Status,
            LeaveRequestDays = request.SelectedDates.Select(d => new LeaveRequestDay
            {
                Date = d
            }).ToList()
        };

        repository.Add(leaveRequest);
        await repository.SaveChangesAsync(cancellationToken);

        return leaveRequest.Id;
    }
}
