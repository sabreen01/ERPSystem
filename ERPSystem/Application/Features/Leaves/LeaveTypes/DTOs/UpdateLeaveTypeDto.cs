namespace ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;

public record UpdateLeaveTypeDto(
    string Name,
    decimal DaysPerYear,
    bool IsPaid,
    bool CarryForward,
    int? MaxCarryDays,
    bool RequiresApproval,
    int MinNoticeDays,
    bool IsActive
);
