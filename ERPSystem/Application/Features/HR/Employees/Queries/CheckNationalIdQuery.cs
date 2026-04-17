using ERPSystem.Domain.Interfaces;
using ERPSystem.Domain.Entities.HR;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Employees.Queries;

public record CheckNationalIdQuery(string NationalId) : IRequest<bool>;
public class CheckNationalIdQueryHandler(IRepository<Employee> repository)
: IRequestHandler<CheckNationalIdQuery, bool>
{
    public async Task<bool> Handle(CheckNationalIdQuery request, CancellationToken cancellationToken)
    => await repository.GetAll(n=>n.NationalId == request.NationalId).AnyAsync(cancellationToken);
}