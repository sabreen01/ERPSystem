using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Commands;

public record UpdateLeaveTypeCommand(
    Guid Id,
    string Name,
    decimal DaysPerYear,
    bool IsPaid,
    bool CarryForward,
    int? MaxCarryDays,
    bool RequiresApproval,
    int MinNoticeDays,
    bool IsActive
) : IRequest<RequestResult<bool>>;

public class UpdateLeaveTypeCommandHandler(IRepository<LeaveType> repository)
    : IRequestHandler<UpdateLeaveTypeCommand, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetById(request.Id);

        if (entity == null)
            return RequestResult<bool>.Failure("Leave type not found.");

        entity.Name = request.Name;
        entity.DaysPerYear = request.DaysPerYear;
        entity.IsPaid = request.IsPaid;
        entity.CarryForward = request.CarryForward;
        entity.MaxCarryDays = request.CarryForward ? request.MaxCarryDays : null;
        entity.RequiresApproval = request.RequiresApproval;
        entity.MinNoticeDays = request.MinNoticeDays;
        entity.IsActive = request.IsActive;

        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<bool>.Success(true, "Leave type updated successfully.");
    }
}
