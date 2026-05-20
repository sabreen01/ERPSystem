using ERPSystem.Domain.Entities.AttendanceManagment;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;

public record GenerateLeaveAttendanceRecordsCommand(
    Guid EmployeeId,
    List<DateTime> RequestedDates
) : IRequest<bool>;

public class GenerateLeaveAttendanceRecordsCommandHandler(
           IRepository<AttendanceEntity> attendanceRepository,
           IRepository<PublicHoliday> holidayRepository)
    : IRequestHandler<GenerateLeaveAttendanceRecordsCommand, bool>
{
    public async Task<bool> Handle(GenerateLeaveAttendanceRecordsCommand request, CancellationToken cancellationToken)
    {
        if (request.RequestedDates == null || !request.RequestedDates.Any())
            return false;

        var dates = request.RequestedDates.Select(d => d.Date).Distinct().ToList();
        var start = dates.Min();
        var end = dates.Max();

       
        var publicHolidays = await holidayRepository
            .GetAll(h => h.IsActive && h.Date >= start && h.Date <= end)
            .Select(h => h.Date.Date)
            .ToListAsync(cancellationToken);

        foreach (var date in dates)
        {
            AttendanceStatus status;

            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                status = AttendanceStatus.DayOff;
            }
            else if (publicHolidays.Contains(date))
            {
                status = AttendanceStatus.Holiday;
            }
            else
            {
                status = AttendanceStatus.OnLeave;
            }

           
            var attendanceRecord = new AttendanceEntity
            {
                EmployeeId = request.EmployeeId,
                Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                Status = status,
                Notes = "Auto-generated from approved leave request"
            };

            attendanceRepository.Add(attendanceRecord);
        }

        await attendanceRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
