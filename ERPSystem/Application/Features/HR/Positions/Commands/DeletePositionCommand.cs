using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Positions.Commands;

public record DeletePositionCommand(Guid Id) : IRequest<bool>;
public class DeletePositionCommandHandler(IRepository<Position> repository)
: IRequestHandler<DeletePositionCommand, bool>
{
    public async Task<bool> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        repository.Delete(request.Id);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}