namespace ERPSystem.Application.Features.Attendance.Overtime.DTOs;

public record PendingOvertimeDto(
    Guid AttendanceId,
    Guid EmployeeId,
    string EmployeeName,
    DateTime Date,
    DateTime? CheckInTime,
    DateTime? CheckOutTime,
    double OvertimeHours
);
