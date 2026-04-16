using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Positions.Commands;

public record CreatePositionCommand(CreatePositionDto Data) : IRequest<Guid>;

public class CreatePositionCommandHandler(IRepository<Position> repository) 
    : IRequestHandler<CreatePositionCommand, Guid>
{
    public async Task<Guid> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Position
        {
            Name = request.Data.Name,
            Code = request.Data.Code,
            DepartmentId = request.Data.DepartmentId,
            MinSalary = request.Data.MinSalary,
            MaxSalary = request.Data.MaxSalary,
            IsActive = true
        };
        
        repository.Add(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}