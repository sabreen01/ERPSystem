using ERPSystem.Application.Features.HR.Positions.Commands;
using ERPSystem.Application.Features.HR.Positions.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Positions.Orchestrators;

public record DeletePositionOrchestrator(Guid Id) : IRequest<RequestResult<bool>>;

public class DeletePositionOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<DeletePositionOrchestrator, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(DeletePositionOrchestrator request, CancellationToken ct)
    {
        var exists = await mediator.Send(new GetPositionByIdQuery(request.Id), ct);
        if (!exists.IsSuccess) 
            return RequestResult<bool>.Failure("Position not found.");
        
        
        // var hasEmployees = await mediator.Send(new CheckPositionHasEmployeesQuery(request.Id));
        // if (hasEmployees) return RequestResult<bool>.Failure("Cannot delete position with active employees.");
        
        var isDeleted = await mediator.Send(new DeletePositionCommand(request.Id), ct);

        return isDeleted 
            ? RequestResult<bool>.Success(true, "Position deleted successfully.")
            : RequestResult<bool>.Failure("Delete operation failed.");
    }
}