using ERPSystem.Application.Features.HR.Departments.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Departments.Queries;

public record GetAllDepartmentsQuery() 
    : IRequest<RequestResult<List<DepartmentResponseDto>>>;
    public class GetAllDepartmentsQueryHandler (IRepository<Department> repository )
    : IRequestHandler<GetAllDepartmentsQuery, RequestResult<List<DepartmentResponseDto>>>
    {
        public async Task<RequestResult<List<DepartmentResponseDto>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await repository.GetAll()
                .Include(d => d.ParentDepartment)
                .Select(d => new DepartmentResponseDto(
                    d.Id, d.Name, d.Code, d.IsActive, 
                    d.ParentDepartment != null ? d.ParentDepartment.Name : null, 
                    d.ParentDepartmentId))
                .ToListAsync(cancellationToken);

            return RequestResult<List<DepartmentResponseDto>>.Success(departments);
        }
            
            
        }
    