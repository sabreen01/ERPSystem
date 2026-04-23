using ERPSystem.Application.Features.Auth.Register.DTOs;
using ERPSystem.Application.Features.Auth.Register.Queries;
using ERPSystem.Application.Features.HR.Employees.Commands;
using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Application.Features.HR.Employees.Queries;
using ERPSystem.Application.Features.HR.Positions.Queries;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Orchestrator;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Employees.Orchestrator;

public record CreateEmployeeOrchestrator(CreateEmployeeDto Data) 
    : IRequest<RequestResult<Guid>>;

public class CreateEmployeeOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<CreateEmployeeOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateEmployeeOrchestrator request, CancellationToken ct)
    {
        if (request.Data.DateOfBirth > DateTime.UtcNow.AddYears(-18))
            return RequestResult<Guid>.Failure("Employee must be 18+ years old.");
        
        if (request.Data.HireDate > DateTime.UtcNow)
            return RequestResult<Guid>.Failure("Hire date cannot be in the future.");
        
        var positionResult = await mediator.Send(new GetPositionByIdQuery(request.Data.PositionId), ct);
        if (!positionResult.IsSuccess) return RequestResult<Guid>.Failure("Position not found.");
        
        if (request.Data.BasicSalary < positionResult.Data.MinSalary || 
            request.Data.BasicSalary > positionResult.Data.MaxSalary)
        {
            return RequestResult<Guid>.Failure($"Salary must be between {positionResult.Data.MinSalary} and {positionResult.Data.MaxSalary}.");
        }
        var idExists = await mediator.Send(new CheckNationalIdQuery(request.Data.NationalId), ct);
        if (idExists) return RequestResult<Guid>.Failure("National ID already registered.");

        
        var registerDto = new RegisterDto 
        {
            UserName = request.Data.PersonalEmail, 
            FirstName = request.Data.FirstName,
            LastName = request.Data.LastName,
            Email = request.Data.PersonalEmail,
            Password = "Emp@" + request.Data.NationalId 
        };

        var registerResult = await mediator.Send(new RegisterOrchestrator(registerDto), ct);
        if (!registerResult.IsSuccess)
        {
            return RequestResult<Guid>.Failure("User creation is failure"+registerResult.Message);
        }
        
        Guid createdUserId = registerResult.Data;
        
        
        var empId = await mediator.Send(new CreateEmployeeCommand(request.Data, createdUserId), ct);
        
        var currentYear = DateTime.UtcNow.Year;
        var balancesResult = await mediator.Send(new InitializeLeaveBalancesOrchestrator(empId, currentYear), ct);
        
        string message = "Employee created successfully.";
        if (balancesResult.IsSuccess)
            message += " " + balancesResult.Message;
        else
            message += " (Warning: Leave balances could not be initialized: " + balancesResult.Message + ")";

        return RequestResult<Guid>.Success(empId, message);
        
    }
}