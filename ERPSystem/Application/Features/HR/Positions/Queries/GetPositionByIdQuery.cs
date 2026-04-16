using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Positions.Queries;

public record GetPositionByIdQuery(Guid Id) : IRequest<RequestResult<PositionResponseDto>>;
public class GetPositionByIdQueryHandler(IRepository<Position> repository)
: IRequestHandler<GetPositionByIdQuery, RequestResult<PositionResponseDto>>
{
    public async Task<RequestResult<PositionResponseDto>> Handle(GetPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var p = await repository.GetAll(x => x.Id == request.Id)
            .Include(p => p.Department)
            .FirstOrDefaultAsync(cancellationToken);

        if (p == null) return RequestResult<PositionResponseDto>.Failure("Position not found");

        var dto = new PositionResponseDto(
            p.Id, p.Name,
            p.Code,
            p.MinSalary,
            p.MaxSalary, 
            p.Department?.Name ?? "No Department", 
            p.IsActive);

        return RequestResult<PositionResponseDto>.Success(dto);
    }
}