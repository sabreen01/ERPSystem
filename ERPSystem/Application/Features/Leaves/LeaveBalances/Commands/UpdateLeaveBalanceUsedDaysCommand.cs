using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;

public record UpdateLeaveBalanceUsedDaysCommand(Guid EmployeeId, Guid LeaveTypeId, int FiscalYear, int DaysToAdd) 
    : IRequest<bool>;

public class UpdateLeaveBalanceUsedDaysCommandHandler(IRepository<LeaveBalance> repository)
    : IRequestHandler<UpdateLeaveBalanceUsedDaysCommand, bool>
{
    public async Task<bool> Handle(UpdateLeaveBalanceUsedDaysCommand request, CancellationToken cancellationToken)
    {
        var balance = await repository
            .GetAll(lb => lb.EmployeeId  == request.EmployeeId && 
                          lb.LeaveTypeId == request.LeaveTypeId &&
                          lb.FiscalYear  == request.FiscalYear)
            .FirstOrDefaultAsync(cancellationToken);

        if (balance == null) return false;

        balance.UsedDays += request.DaysToAdd;
        
        repository.Update(balance);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
