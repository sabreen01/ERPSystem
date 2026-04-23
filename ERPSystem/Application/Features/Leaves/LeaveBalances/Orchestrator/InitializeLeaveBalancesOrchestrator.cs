using ERPSystem.Application.Features.HR.Employees.Queries;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Commands;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Queries;
using ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Orchestrator;

public record InitializeLeaveBalancesOrchestrator(Guid EmployeeId, int FiscalYear) 
    : IRequest<RequestResult<bool>>;

public class InitializeLeaveBalancesOrchestratorHandler(IMediator mediator)
    : IRequestHandler<InitializeLeaveBalancesOrchestrator, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(
        InitializeLeaveBalancesOrchestrator request, CancellationToken cancellationToken)
    {
       
        var employeeResult = await mediator.Send(new GetEmployeeByIdQuery(request.EmployeeId), cancellationToken);
        if (!employeeResult.IsSuccess)
            return RequestResult<bool>.Failure("Employee not found.");

        if (!employeeResult.Data.IsActive)
            return RequestResult<bool>.Failure("Employee is not active.");

        
        var existingBalances = await mediator.Send(
            new CheckLeaveBalanceForEmployeeQuery(request.EmployeeId, request.FiscalYear), cancellationToken);
            
        if (existingBalances)
            return RequestResult<bool>.Failure($"Leave balances for year {request.FiscalYear} already exist for this employee.");

       
        var leaveTypesResult = await mediator.Send(new GetAllLeaveTypesQuery(), cancellationToken);
        
        if (!leaveTypesResult.IsSuccess || !leaveTypesResult.Data.Any())
            return RequestResult<bool>.Failure("No active leave types found. Please create leave types first.");
            
        var leaveTypes = leaveTypesResult.Data;
        
        var hireDate = employeeResult.Data.HireDate;
        var yearStart = new DateTime(request.FiscalYear, 1, 1);

        decimal prorationFactor = 1.0m;

        if (hireDate.Year == request.FiscalYear && hireDate > yearStart)
        {
            int remainingMonths = 12 - hireDate.Month + 1;
            prorationFactor = (decimal)remainingMonths / 12;
        }

       
        foreach (var leaveType in leaveTypes)
        {
            var openingBalance = Math.Round(leaveType.DaysPerYear * prorationFactor, 1);

            await mediator.Send(new CreateLeaveBalanceCommand(
                request.EmployeeId,
                leaveType.Id,
                request.FiscalYear,
                openingBalance
            ), cancellationToken);
        }

        return RequestResult<bool>.Success(true, 
            $"Leave balances initialized successfully for {leaveTypes.Count} leave types (Proration: {prorationFactor:P0}).");
    }
}
