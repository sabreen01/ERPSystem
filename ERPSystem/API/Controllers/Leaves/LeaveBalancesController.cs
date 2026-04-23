using ERPSystem.Application.Features.Leaves.LeaveBalances.DTOs;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Orchestrator;
using ERPSystem.Application.Features.Leaves.LeaveBalances.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.Leaves;

[ApiController]
[Route("[controller]")]
public class LeaveBalancesController(IMediator mediator) : BaseController
{
    //b3melha le el hr aw le tenant 3ndh employees Already mawgodeen 
    [HttpPost]
    public async Task<ActionResult> Initialize([FromBody] InitializeLeaveBalanceDto dto)
    {
        return HandleResult(await mediator.Send(
            new InitializeLeaveBalancesOrchestrator(dto.EmployeeId, dto.FiscalYear)));
    }

    [HttpPost("BulkInitialize/{fiscalYear}")]
    public async Task<ActionResult> BulkInitialize(int fiscalYear)
    {
        return HandleResult(await mediator.Send(
            new BulkInitializeLeaveBalancesOrchestrator(fiscalYear)));
    }

    [HttpGet("GetByEmployee/{employeeId}/{fiscalYear}")]
    public async Task<ActionResult> GetByEmployee(Guid employeeId, int fiscalYear)
    {
        return HandleResult(await mediator.Send(
            new GetEmployeeLeaveBalancesQuery(employeeId, fiscalYear)));
    }
}

