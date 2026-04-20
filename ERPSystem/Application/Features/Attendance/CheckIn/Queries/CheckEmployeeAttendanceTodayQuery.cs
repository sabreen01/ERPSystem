using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;

namespace ERPSystem.Application.Features.Attendance.CheckIn.Queries;

public record CheckEmployeeAttendanceTodayQuery(Guid EmployeeId) : IRequest<bool>;

public class CheckEmployeeAttendanceTodayQueryHandler(IRepository<AttendanceEntity> repository) 
    : IRequestHandler<CheckEmployeeAttendanceTodayQuery, bool>
{
    public async Task<bool> Handle(CheckEmployeeAttendanceTodayQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var hasCheckedIn = await repository.GetAll(a => a.EmployeeId == request.EmployeeId)
            .AnyAsync(a => a.Date == today, cancellationToken);

        return hasCheckedIn;
    }
}
