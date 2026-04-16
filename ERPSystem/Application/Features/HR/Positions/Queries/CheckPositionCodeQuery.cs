using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Positions.Queries;

public record CheckPositionCodeQuery(string Code) : IRequest<bool>;

public class CheckPositionCodeHandler(IRepository<Position> repository) 
    : IRequestHandler<CheckPositionCodeQuery, bool>
{
    public async Task<bool> Handle(CheckPositionCodeQuery request, CancellationToken ct)
        => await repository.GetAll(p => p.Code == request.Code).AnyAsync(ct);
}