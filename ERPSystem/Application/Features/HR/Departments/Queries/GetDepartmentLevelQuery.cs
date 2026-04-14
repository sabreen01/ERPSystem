using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Departments.Queries;

public record GetDepartmentLevelQuery(Guid DepartmentId) : IRequest<int>;
public class GetDepartmentLevelQueryHandler(IRepository<Department> repository)
    : IRequestHandler<GetDepartmentLevelQuery , int>
{
    public async Task<int> Handle(GetDepartmentLevelQuery query, CancellationToken cancellationToken)
    {
        var department = await repository.GetAll(d => d.Id == query.DepartmentId)
            .Include(d => d.ParentDepartment)
            .ThenInclude(p => p.ParentDepartment)
            .FirstOrDefaultAsync(cancellationToken);
        if (department is null) return 0;
        int level = 1;
        var current = department;
        while (current.ParentDepartmentId != null)
        {
            level++;
            current = current.ParentDepartment;
        }
        return level;
    }
}
