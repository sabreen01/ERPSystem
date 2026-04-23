using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;

public record CreateLeaveBalanceCommand(
    Guid EmployeeId,
    Guid LeaveTypeId,
    int FiscalYear,
    decimal OpeningBalance
) : IRequest<Guid>;

public class CreateLeaveBalanceCommandHandler(IRepository<LeaveBalance> repository)
    : IRequestHandler<CreateLeaveBalanceCommand, Guid>
{
    public async Task<Guid> Handle(CreateLeaveBalanceCommand request, CancellationToken cancellationToken)
    {
        var entity = new LeaveBalance
        {
            EmployeeId = request.EmployeeId,
            LeaveTypeId = request.LeaveTypeId,
            FiscalYear = request.FiscalYear,
            OpeningBalance = request.OpeningBalance,
            UsedDays = 0,
            CarriedForward = 0,
            Adjustment = 0
        };

        repository.Add(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
