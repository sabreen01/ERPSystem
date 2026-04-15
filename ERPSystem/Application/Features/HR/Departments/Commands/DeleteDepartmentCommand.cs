using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Departments.Commands;

public record DeleteDepartmentCommand(Guid Id) : IRequest<bool>;
public class  DeleteDepartmentHandler (IRepository<Department> repository)
    : IRequestHandler<DeleteDepartmentCommand, bool>
{
    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
       
        repository.Delete(request.Id);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
