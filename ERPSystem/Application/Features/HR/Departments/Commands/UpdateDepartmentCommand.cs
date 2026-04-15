using ERPSystem.Application.Features.HR.Departments.DTOs;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Departments.Commands;

public record UpdateDepartmentCommand(Guid Id, UpdateDepartmentDto Data)
    : IRequest<bool>;

public class UpdateDepartmentCommandHandler(IRepository<Department> repository)
    : IRequestHandler<UpdateDepartmentCommand, bool>
{
    public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var dept = await repository.GetById(request.Id);
        if (dept == null)
        {
            return false;
        }
        
        dept.Name = request.Data.Name;
        dept.Code = request.Data.Code;
        dept.IsActive = request.Data.IsActive;
        dept.ParentDepartmentId = request.Data.ParentId;
        
        repository.Update(dept);
        await repository.SaveChangesAsync(cancellationToken);
        return true;

    }
}
