using ERPSystem.Application.Features.HR.Departments.Commands;
using ERPSystem.Application.Features.HR.Departments.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Departments.Orchestrator;

public record DeleteDepartmentOrchestrator(Guid Id)
    : IRequest<RequestResult<bool>>;

public class DeleteDepartmentOrchestratorHandler(IMediator mediator)
    : IRequestHandler<DeleteDepartmentOrchestrator, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(DeleteDepartmentOrchestrator request, CancellationToken cancellationToken)
    {
        var isExist = await mediator.Send(new GetDepartmentByIdQuery(request.Id));
        if (!isExist.IsSuccess)
        {
            return RequestResult<bool>.Failure("Department not found");
        }
        var hasChildren = await mediator.Send(new CheckDepartmentHasChildrenQuery(request.Id), cancellationToken);
        if (hasChildren) 
            return RequestResult<bool>.Failure("Cannot delete! This department has sub-departments linked to it.");
        
        var isDeleted = await mediator.Send(new DeleteDepartmentCommand(request.Id), cancellationToken);
        return isDeleted 
            ? RequestResult<bool>.Success(true, "Department Deleted successfully.")
            : RequestResult<bool>.Failure("Delete failed.");
    }
}

