using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;

public record GetLeaveRequestByIdQuery(Guid Id) : IRequest<LeaveRequest?>;

public class GetLeaveRequestByIdQueryHandler(IRepository<LeaveRequest> repository)
    : IRequestHandler<GetLeaveRequestByIdQuery, LeaveRequest?>
{
    public async Task<LeaveRequest?> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll(lr => lr.Id == request.Id)
            .Include(lr => lr.LeaveRequestDays)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
