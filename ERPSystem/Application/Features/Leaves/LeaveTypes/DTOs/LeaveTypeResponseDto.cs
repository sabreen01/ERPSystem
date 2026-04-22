namespace ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;

public record LeaveTypeResponseDto(
    Guid Id,
    string Name,
    string Code,
    decimal DaysPerYear,
    bool IsPaid,
    bool CarryForward,
    int? MaxCarryDays,
    bool RequiresApproval,
    int MinNoticeDays,
    bool IsActive
);
