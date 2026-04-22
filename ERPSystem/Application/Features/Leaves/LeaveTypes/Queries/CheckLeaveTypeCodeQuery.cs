using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;

public record CheckLeaveTypeCodeQuery(string Code) : IRequest<bool>;

public class CheckLeaveTypeCodeQueryHandler(IRepository<LeaveType> repository)
    : IRequestHandler<CheckLeaveTypeCodeQuery, bool>
{
    public async Task<bool> Handle(CheckLeaveTypeCodeQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll(lt => lt.Code == request.Code.ToUpper())
            .AnyAsync(cancellationToken);
    }
}
