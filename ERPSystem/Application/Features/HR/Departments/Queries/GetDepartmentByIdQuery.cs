using ERPSystem.Application.Features.HR.Departments.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Departments.Queries;

public record GetDepartmentByIdQuery(Guid Id) 
    : IRequest<RequestResult<DepartmentResponseDto>>;
public class GetDepartmentByIdQueryHandler(IRepository<Department> repository)
    :IRequestHandler<GetDepartmentByIdQuery, RequestResult<DepartmentResponseDto>>
{
    public async Task<RequestResult<DepartmentResponseDto>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var d = await repository.GetAll(x => x.Id == request.Id)
            .Include(d => d.ParentDepartment)
            .FirstOrDefaultAsync(cancellationToken);

        if (d == null) 
            return RequestResult<DepartmentResponseDto>.Failure("Department not found");

        var dto = new DepartmentResponseDto(d.Id, d.Name, d.Code, d.IsActive, 
            d.ParentDepartment?.Name, d.ParentDepartmentId);

        return RequestResult<DepartmentResponseDto>.Success(dto);
    }
}
    