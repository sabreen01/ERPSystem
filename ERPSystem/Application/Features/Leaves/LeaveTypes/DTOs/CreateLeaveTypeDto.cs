namespace ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;

public record CreateLeaveTypeDto(
    string Name,
    string Code,
    decimal DaysPerYear,
    bool IsPaid,
    bool CarryForward,
    int? MaxCarryDays,
    bool RequiresApproval,
    int MinNoticeDays
);
