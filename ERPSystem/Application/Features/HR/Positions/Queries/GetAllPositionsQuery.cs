using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Positions.Queries;

public record GetAllPositionsQuery(): IRequest<RequestResult<List<PositionResponseDto>>>;

public class GetAllPositionsQueryHandler(IRepository<Position> repository)
:IRequestHandler<GetAllPositionsQuery, RequestResult<List<PositionResponseDto>>>
{
    public async Task<RequestResult<List<PositionResponseDto>>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await repository.GetAll()
            .Include(p => p.Department)
            .Select(p => new PositionResponseDto(
                p.Id,
                p.Name,
                p.Code,
                p.MinSalary,
                p.MaxSalary,
                p.Department != null ? p.Department.Name : "No Department", 
                p.IsActive))
            .ToListAsync(cancellationToken);
        return RequestResult<List<PositionResponseDto>>.Success(positions);
        
    }
}