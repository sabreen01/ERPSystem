using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Employees.Queries;

public record GetAllEmployeesQuery(
    string? SearchTerm = null,
    Guid? DepartmentId = null,
    Guid? BranchId = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<RequestResult<PagedResult<EmployeeResponseDto>>>;

public class GetAllEmployeesQueryHandler(IRepository<Employee> repository) 
    : IRequestHandler<GetAllEmployeesQuery, RequestResult<PagedResult<EmployeeResponseDto>>>
{
    public async Task<RequestResult<PagedResult<EmployeeResponseDto>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = repository.GetAll();

       
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(e => 
                e.FirstName.ToLower().Contains(search) || 
                e.LastName.ToLower().Contains(search) || 
                e.EmployeeNo.ToLower().Contains(search) || 
                (e.PersonalEmail != null && e.PersonalEmail.ToLower().Contains(search)));
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == request.DepartmentId.Value);
        }
        
        if (request.BranchId.HasValue)
        {
            query = query.Where(e => e.BranchId == request.BranchId.Value);
        }

       
        var totalCount = await query.CountAsync(cancellationToken);

       
        var employees = await query
            .Include(e => e.Position)
            .Include(e => e.Department)
            .Include(e => e.Branch)
            .OrderByDescending(e => e.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmployeeResponseDto(
                e.Id,
                e.EmployeeNo,
                e.FirstName,
                e.LastName,
                e.PersonalEmail,
                e.Position != null ? e.Position.Name : null,
                e.Department != null ? e.Department.Name : null,
                e.Branch != null ? e.Branch.Name : null,
                e.IsActive
            ))
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<EmployeeResponseDto>(employees, totalCount, request.PageNumber, request.PageSize);

        return RequestResult<PagedResult<EmployeeResponseDto>>.Success(pagedResult);
    }
}
