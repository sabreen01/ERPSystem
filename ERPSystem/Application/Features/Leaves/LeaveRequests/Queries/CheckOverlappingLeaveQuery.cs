using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;

public record CheckOverlappingLeaveQuery(Guid EmployeeId, List<DateTime> RequestedDates) 
    : IRequest<bool>;

public class CheckOverlappingLeaveQueryHandler(IRepository<LeaveRequestDay> repository)
    : IRequestHandler<CheckOverlappingLeaveQuery, bool>
{
    public async Task<bool> Handle(CheckOverlappingLeaveQuery request, CancellationToken cancellationToken)
    {
        if (request.RequestedDates == null || !request.RequestedDates.Any()) return false;

        var dates = request.RequestedDates.Select(d => d.Date).ToList();

        return await repository.GetAll(lrd => 
                lrd.LeaveRequest.EmployeeId == request.EmployeeId &&
                lrd.LeaveRequest.Status != LeaveRequestStatus.Rejected && 
                lrd.LeaveRequest.Status != LeaveRequestStatus.Cancelled &&
                dates.Contains(lrd.Date))
            .AnyAsync(cancellationToken);
    }
}
