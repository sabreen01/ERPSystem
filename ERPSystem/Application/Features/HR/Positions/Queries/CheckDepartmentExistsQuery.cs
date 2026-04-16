using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Positions.Queries;

public record CheckDepartmentExistsQuery(Guid DeptId) : IRequest<bool>;
public class CheckDepartmentExistsHandler(IRepository<Department> repository) 
    : IRequestHandler<CheckDepartmentExistsQuery, bool>
{
    public async Task<bool> Handle(CheckDepartmentExistsQuery request, CancellationToken ct)
        => await repository.GetAll(d => d.Id == request.DeptId).AnyAsync(ct);
}