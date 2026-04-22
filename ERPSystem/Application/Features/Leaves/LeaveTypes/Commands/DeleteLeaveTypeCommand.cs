using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Commands;

public record DeleteLeaveTypeCommand(Guid Id) : IRequest<RequestResult<bool>>;

public class DeleteLeaveTypeCommandHandler(IRepository<LeaveType> repository)
    : IRequestHandler<DeleteLeaveTypeCommand, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetById(request.Id);

        if (entity == null)
            return RequestResult<bool>.Failure("Leave type not found.");

        repository.Delete(entity.Id);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<bool>.Success(true, "Leave type deleted successfully.");
    }
}
