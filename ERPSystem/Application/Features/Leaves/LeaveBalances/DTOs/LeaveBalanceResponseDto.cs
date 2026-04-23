namespace ERPSystem.Application.Features.Leaves.LeaveBalances.DTOs;

public record LeaveBalanceResponseDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    Guid LeaveTypeId,
    string LeaveTypeName,
    int FiscalYear,
    decimal OpeningBalance,
    decimal UsedDays,
    decimal CarriedForward,
    decimal Adjustment,
    decimal RemainingBalance
);
