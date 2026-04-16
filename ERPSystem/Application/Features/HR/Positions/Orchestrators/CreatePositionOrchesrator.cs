using ERPSystem.Application.Features.HR.Positions.Commands;
using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Application.Features.HR.Positions.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Positions.Orchestrators;

public record CreatePositionOrchestrator(CreatePositionDto Data) : IRequest<RequestResult<Guid>>;

public class CreatePositionOrchestratorHandler(IMediator _mediator) 
    : IRequestHandler<CreatePositionOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreatePositionOrchestrator request, CancellationToken cancellationToken)
    {
        if (request.Data.MinSalary >= request.Data.MaxSalary)
        {
            return RequestResult<Guid>.Failure("Minimum salary must be less than maximum salary.");
        }
        
        var codeExists = await _mediator.Send(new CheckPositionCodeQuery(request.Data.Code));
        if (codeExists)
            return RequestResult<Guid>.Failure("Position code already exists.");
        
        var deptExists = await _mediator.Send(new CheckDepartmentExistsQuery(request.Data.DepartmentId));
        if (!deptExists)
            return RequestResult<Guid>.Failure("Selected department does not exist.");
        
        var id = await _mediator.Send(new CreatePositionCommand(request.Data));
        
        return RequestResult<Guid>.Success(id, "Position created successfully.");
    }
}