using ERPSystem.Application.Features.Leaves.LeaveTypes.Commands;
using ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Orchestrator;

public record CreateLeaveTypeOrchestrator(
    string Name,
    string Code,
    decimal DaysPerYear,
    bool IsPaid,
    bool CarryForward,
    int? MaxCarryDays,
    bool RequiresApproval,
    int MinNoticeDays
) : IRequest<RequestResult<Guid>>;

public class CreateLeaveTypeOrchestratorHandler(IMediator mediator)
    : IRequestHandler<CreateLeaveTypeOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateLeaveTypeOrchestrator request, CancellationToken cancellationToken)
    {
       
        var codeExists = await mediator.Send(new CheckLeaveTypeCodeQuery(request.Code), cancellationToken);
        if (codeExists)
        {
            return RequestResult<Guid>.Failure($"Leave type with code '{request.Code.ToUpper()}' already exists.");
        }

        if (request.CarryForward && (request.MaxCarryDays == null || request.MaxCarryDays <= 0))
        {
            return RequestResult<Guid>.Failure("Max carry days must be provided when carry forward is enabled.");
        }
        
        var id = await mediator.Send(new CreateLeaveTypeCommand(
            request.Name,
            request.Code,
            request.DaysPerYear,
            request.IsPaid,
            request.CarryForward,
            request.MaxCarryDays,
            request.RequiresApproval,
            request.MinNoticeDays
        ), cancellationToken);

        return RequestResult<Guid>.Success(id, "Leave type created successfully.");
    }
}
