using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Commands;

public record CreateLeaveTypeCommand(
    string Name,
    string Code,
    decimal DaysPerYear,
    bool IsPaid,
    bool CarryForward,
    int? MaxCarryDays,
    bool RequiresApproval,
    int MinNoticeDays
) : IRequest<Guid>;

public class CreateLeaveTypeCommandHandler(IRepository<LeaveType> repository)
    : IRequestHandler<CreateLeaveTypeCommand, Guid>
{
    public async Task<Guid> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new LeaveType
        {
            Name = request.Name,
            Code = request.Code.ToUpper(),
            DaysPerYear = request.DaysPerYear,
            IsPaid = request.IsPaid,
            CarryForward = request.CarryForward,
            MaxCarryDays = request.CarryForward ? request.MaxCarryDays : null,
            RequiresApproval = request.RequiresApproval,
            MinNoticeDays = request.MinNoticeDays
        };

        repository.Add(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
