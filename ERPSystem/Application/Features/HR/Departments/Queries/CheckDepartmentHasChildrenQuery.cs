using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Departments.Queries;

public record CheckDepartmentHasChildrenQuery(Guid Id) : IRequest<bool>;

public class CheckDepartmentHasChildrenHandler(IRepository<Department> repository)
: IRequestHandler<CheckDepartmentHasChildrenQuery, bool>
{
    public async Task<bool> Handle(CheckDepartmentHasChildrenQuery request, CancellationToken cancellationToken)
    => await repository.GetAll(d => d.ParentDepartmentId == request.Id).AnyAsync(cancellationToken);
}