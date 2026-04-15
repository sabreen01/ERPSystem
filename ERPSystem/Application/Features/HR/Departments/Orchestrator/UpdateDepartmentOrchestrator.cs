using ERPSystem.Application.Features.HR.Departments.Commands;
using ERPSystem.Application.Features.HR.Departments.DTOs;
using ERPSystem.Application.Features.HR.Departments.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Departments.Orchestrator;

public record UpdateDepartmentOrchestrator(Guid Id, UpdateDepartmentDto Data)
    : IRequest<RequestResult<bool>>;

public class UpdateDepartmentOrchestratorHandler(IMediator  mediator)
    : IRequestHandler<UpdateDepartmentOrchestrator, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(UpdateDepartmentOrchestrator request, CancellationToken cancellationToken)
    {
        var IsExist = await mediator.Send(new GetDepartmentByIdQuery(request.Id));
        if (!IsExist.IsSuccess)
        {
            return RequestResult<bool>.Failure("Department not found");
        }

        if (request.Id == request.Data.ParentId)
        {
            return RequestResult<bool>.Failure("A department cannot be its own parent");
        }

        if (request.Data.ParentId.HasValue)
        {
            var parentLevel = await mediator.Send(new GetDepartmentLevelQuery(request.Data.ParentId.Value));
            if (parentLevel >= 3)
            {
                return RequestResult<bool>.Failure("Hierarchy limit exceeded! Target parent is already at level 3.");
            }
        }
        
        var isUpdated = await mediator.Send(new UpdateDepartmentCommand(request.Id, request.Data));
        return isUpdated 
            ? RequestResult<bool>.Success(true, "Department updated successfully.")
            : RequestResult<bool>.Failure("Update failed.");
        
    }
}
