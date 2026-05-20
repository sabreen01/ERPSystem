namespace ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;

public record ApplyForLeaveDto(
    Guid EmployeeId,
    Guid LeaveTypeId,
    List<DateTime> SelectedDates,
    string? Reason
);
