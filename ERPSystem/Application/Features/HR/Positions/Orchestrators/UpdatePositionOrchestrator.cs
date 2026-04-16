using ERPSystem.Application.Features.HR.Positions.Commands;
using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Application.Features.HR.Positions.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Positions.Orchestrators;

public record UpdatePositionOrchestrator(Guid Id, UpdatePositionDto Data) 
    : IRequest<RequestResult<bool>>;
public class UpdatePositionOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<UpdatePositionOrchestrator, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(UpdatePositionOrchestrator request, CancellationToken cancellationToken)
    {
        var positionExists = await mediator.Send(new GetPositionByIdQuery(request.Id), cancellationToken);
        if (!positionExists.IsSuccess) 
            return RequestResult<bool>.Failure("Position not found.");
        
        if (request.Data.MinSalary >= request.Data.MaxSalary)
            return RequestResult<bool>.Failure("Update failed: Minimum salary must be less than maximum.");
        
        var deptExists = await mediator.Send(new CheckDepartmentExistsQuery(request.Data.DepartmentId), cancellationToken);
        if (!deptExists) return RequestResult<bool>.Failure("The selected department does not exist.");
        
        var isUpdated = await mediator.Send(new UpdatePositionCommand(request.Id, request.Data), cancellationToken);
        
        return isUpdated 
            ? RequestResult<bool>.Success(true, "Position updated successfully.")
            : RequestResult<bool>.Failure("An error occurred during update.");
    }
}