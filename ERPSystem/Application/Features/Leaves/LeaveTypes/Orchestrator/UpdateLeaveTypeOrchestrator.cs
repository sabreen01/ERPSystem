using ERPSystem.Application.Features.Leaves.LeaveTypes.Commands;
using ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;
using ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Orchestrator;

public record UpdateLeaveTypeOrchestrator(Guid Id, UpdateLeaveTypeDto Dto) : IRequest<RequestResult<bool>>;

public class UpdateLeaveTypeOrchestratorHandler(IMediator mediator)
    : IRequestHandler<UpdateLeaveTypeOrchestrator, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(UpdateLeaveTypeOrchestrator request, CancellationToken cancellationToken)
    {
        
        var existing = await mediator.Send(new GetLeaveTypeByIdQuery(request.Id), cancellationToken);
        if (!existing.IsSuccess)
        {
            return RequestResult<bool>.Failure("Leave type not found.");
        }

       
        if (request.Dto.CarryForward && (request.Dto.MaxCarryDays == null || request.Dto.MaxCarryDays <= 0))
        {
            return RequestResult<bool>.Failure("Max carry days must be provided when carry forward is enabled.");
        }

       
        return await mediator.Send(new UpdateLeaveTypeCommand(
            request.Id,
            request.Dto.Name,
            request.Dto.DaysPerYear,
            request.Dto.IsPaid,
            request.Dto.CarryForward,
            request.Dto.MaxCarryDays,
            request.Dto.RequiresApproval,
            request.Dto.MinNoticeDays,
            request.Dto.IsActive
        ), cancellationToken);
    }
}
