using ERPSystem.Application.Features.HR.Employees.Commands;
using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Application.Features.HR.Positions.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Employees.Orchestrator;

public record UpdateEmployeeOrchestrator(Guid Id, UpdateEmployeeDto Data) : IRequest<RequestResult<Guid>>;

public class UpdateEmployeeOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<UpdateEmployeeOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(UpdateEmployeeOrchestrator request, CancellationToken cancellationToken)
    {
        if (request.Data.DateOfBirth > DateTime.UtcNow.AddYears(-18))
            return RequestResult<Guid>.Failure("Employee must be 18+ years old.");
        
        var positionResult = await mediator.Send(new GetPositionByIdQuery(request.Data.PositionId), cancellationToken);
        if (!positionResult.IsSuccess) return RequestResult<Guid>.Failure("Position not found.");
        
        if (request.Data.BasicSalary < positionResult.Data.MinSalary || 
            request.Data.BasicSalary > positionResult.Data.MaxSalary)
        {
            return RequestResult<Guid>.Failure($"Salary must be between {positionResult.Data.MinSalary} and {positionResult.Data.MaxSalary}.");
        }
        
        var success = await mediator.Send(new UpdateEmployeeCommand(request.Id, request.Data), cancellationToken);

        if (!success) return RequestResult<Guid>.Failure("Employee not found.");

        return RequestResult<Guid>.Success(request.Id, "Employee updated successfully.");
    }
}
