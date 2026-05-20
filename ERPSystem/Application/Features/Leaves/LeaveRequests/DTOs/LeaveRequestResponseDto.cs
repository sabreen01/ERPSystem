using ERPSystem.Domain.Enums;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;

public record LeaveRequestResponseDto(
    Guid Id,
    Guid EmployeeId,
    Guid LeaveTypeId,
    DateTime StartDate,
    DateTime EndDate,
    int DaysCount,
    string? Reason,
    LeaveRequestStatus Status,
    List<DateTime> RequestedDates,
    DateTime? CreatedAt
);
