using ERPSystem.Application.Features.Leaves.LeaveBalances.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveBalances.Queries;

public record GetEmployeeLeaveBalancesQuery(Guid EmployeeId, int FiscalYear) 
    : IRequest<RequestResult<List<LeaveBalanceResponseDto>>>;

public class GetEmployeeLeaveBalancesQueryHandler(IRepository<LeaveBalance> repository)
    : IRequestHandler<GetEmployeeLeaveBalancesQuery, RequestResult<List<LeaveBalanceResponseDto>>>
{
    public async Task<RequestResult<List<LeaveBalanceResponseDto>>> Handle(
        GetEmployeeLeaveBalancesQuery request, CancellationToken cancellationToken)
    {
        var balances = await repository
            .GetAll(lb => lb.EmployeeId == request.EmployeeId && lb.FiscalYear == request.FiscalYear)
            .Include(lb => lb.Employee)
            .Include(lb => lb.LeaveType)
            .Select(lb => new LeaveBalanceResponseDto(
                lb.Id,
                lb.EmployeeId,
                lb.Employee != null ? $"{lb.Employee.FirstName} {lb.Employee.LastName}" : "Unknown",
                lb.LeaveTypeId,
                lb.LeaveType != null ? lb.LeaveType.Name : "Unknown",
                lb.FiscalYear,
                lb.OpeningBalance,
                lb.UsedDays,
                lb.CarriedForward,
                lb.Adjustment,
                lb.OpeningBalance + lb.CarriedForward + lb.Adjustment - lb.UsedDays
            ))
            .ToListAsync(cancellationToken);

        return RequestResult<List<LeaveBalanceResponseDto>>.Success(balances);
    }
}
