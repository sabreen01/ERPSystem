using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Positions.Commands;

public record UpdatePositionCommand(Guid Id, UpdatePositionDto Data) : IRequest<bool>;
public class UpdatePositionCommandHandler(IRepository<Position> repository)
: IRequestHandler<UpdatePositionCommand, bool>
{
    public async Task<bool> Handle(UpdatePositionCommand request, CancellationToken ct)
    {
        var position = await repository.GetById(request.Id);

        position.Name = request.Data.Name;
        position.MinSalary = request.Data.MinSalary;
        position.MaxSalary = request.Data.MaxSalary;
        position.DepartmentId = request.Data.DepartmentId;
        position.IsActive = request.Data.IsActive;

        repository.Update(position);
        await repository.SaveChangesAsync(ct);
        return true;
    }
}