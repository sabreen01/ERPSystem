using ERPSystem.Application.Features.HR.Employees.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Orchestrator;

public record BulkInitializeLeaveBalancesOrchestrator(int FiscalYear) 
    : IRequest<RequestResult<string>>;

public class BulkInitializeLeaveBalancesOrchestratorHandler(IMediator mediator)
    : IRequestHandler<BulkInitializeLeaveBalancesOrchestrator, RequestResult<string>>
{
    public async Task<RequestResult<string>> Handle(
        BulkInitializeLeaveBalancesOrchestrator request, CancellationToken cancellationToken)
    {
      
        var employeesResult = await mediator.Send(new GetAllEmployeesQuery(PageSize: 1000), cancellationToken);
        if (!employeesResult.IsSuccess || !employeesResult.Data.Items.Any())
        {
            return RequestResult<string>.Failure("No employees found in the system.");
        }

        int successCount = 0;
        int skipCount = 0;
        int failCount = 0;
        
        foreach (var emp in employeesResult.Data.Items.Where(e => e.IsActive))
        {
            var initResult = await mediator.Send(
                new InitializeLeaveBalancesOrchestrator(emp.Id, request.FiscalYear), cancellationToken);

            if (initResult.IsSuccess)
                successCount++;
            else if (initResult.Message.Contains("already exist"))
                skipCount++; // They already have balances
            else
                failCount++;
        }

        return RequestResult<string>.Success(
            "Bulk initialization completed.", 
            $"Successfully initialized: {successCount} employees. Skipped (already exist): {skipCount}. Failed: {failCount}."
        );
    }
}
