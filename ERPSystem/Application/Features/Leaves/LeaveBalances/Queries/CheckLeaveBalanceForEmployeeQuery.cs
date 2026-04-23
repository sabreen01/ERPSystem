using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Queries;

public record CheckLeaveBalanceForEmployeeQuery(Guid EmployeeId, int FiscalYear) : IRequest<bool>;

public class CheckLeaveBalanceForEmployeeQueryHandler(IRepository<LeaveBalance> repository):
    IRequestHandler<CheckLeaveBalanceForEmployeeQuery, bool>
{
    public async Task<bool> Handle(CheckLeaveBalanceForEmployeeQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAll(lb => lb.EmployeeId == request.EmployeeId && lb.FiscalYear == request.FiscalYear)
            .AnyAsync(cancellationToken);
    }
}
