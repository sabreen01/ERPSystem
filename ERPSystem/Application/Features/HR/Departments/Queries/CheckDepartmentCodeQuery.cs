using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Departments.Queries;

public record CheckDepartmentCodeQuery(string Code) : IRequest<bool>;

public class CheckDepartmentCodeQueryHandler(IRepository<Department> repository)
:IRequestHandler<CheckDepartmentCodeQuery, bool>
{
    public async Task<bool> Handle(CheckDepartmentCodeQuery query, CancellationToken cancellationToken)
  
      =>  await repository.GetAll(d=>d.Code==query.Code).AnyAsync(cancellationToken);
    
}