using ERPSystem.Domain.Entities.AttendanceManagment;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Commands;

public record DeleteLeaveAttendanceRecordsCommand(Guid EmployeeId, List<DateTime> RequestedDates) : IRequest<bool>;

public class DeleteLeaveAttendanceRecordsCommandHandler(IRepository<AttendanceEntity> attendanceRepository)
    : IRequestHandler<DeleteLeaveAttendanceRecordsCommand, bool>
{
    public async Task<bool> Handle(DeleteLeaveAttendanceRecordsCommand request, CancellationToken cancellationToken)
    {
        if (request.RequestedDates == null || !request.RequestedDates.Any()) return false;

        var dates = request.RequestedDates.Select(d => d.Date).ToList();

        var recordsToDelete = await attendanceRepository
            .GetAll(a => a.EmployeeId == request.EmployeeId && dates.Contains(a.Date.Date))
            .ToListAsync(cancellationToken);

        foreach (var record in recordsToDelete)
        {
            attendanceRepository.Delete(record.Id);
        }

        await attendanceRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
