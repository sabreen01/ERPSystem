using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Departments.Commands;

public record CreateDepartmentCommand(string Name 
    , string Code 
    ,Guid? ParentId)
    :IRequest<Guid>;


public class CreateDepartmentCommandHandler (IRepository<Department> repository) 
    :IRequestHandler<CreateDepartmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Department
        {
            Name = request.Name, 
            Code = request.Code, 
            ParentDepartmentId = request.ParentId
        };
        repository.Add(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
