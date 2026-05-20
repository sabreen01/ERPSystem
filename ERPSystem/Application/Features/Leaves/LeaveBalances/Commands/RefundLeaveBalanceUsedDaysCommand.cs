using ERPSystem.Application.Features.Leaves.LeaveBalances.Queries;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;

public record RefundLeaveBalanceUsedDaysCommand(Guid EmployeeId, Guid LeaveTypeId, int FiscalYear, int DaysToRefund) : IRequest<bool>;

public class RefundLeaveBalanceUsedDaysCommandHandler(IRepository<LeaveBalance> repository)
    : IRequestHandler<RefundLeaveBalanceUsedDaysCommand, bool>
{
    public async Task<bool> Handle(RefundLeaveBalanceUsedDaysCommand request, CancellationToken cancellationToken)
    {
        var balance = await repository
            .GetAll(b => 
                b.EmployeeId == request.EmployeeId && 
                b.LeaveTypeId == request.LeaveTypeId &&
                b.FiscalYear == request.FiscalYear)
            .FirstOrDefaultAsync(cancellationToken);

        if (balance == null) return false;

        balance.UsedDays -= request.DaysToRefund;

        // Ensure we don't go below 0 (just in case)
        if (balance.UsedDays < 0) balance.UsedDays = 0;

        repository.Update(balance);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
