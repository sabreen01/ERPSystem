using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;

namespace ERPSystem.Application.Features.Attendance.CheckOut.Queries;

public record GetTodayAttendanceQuery(Guid EmployeeId) : IRequest<AttendanceEntity?>;

public class GetTodayAttendanceQueryHandler(IRepository<AttendanceEntity> repository) 
    : IRequestHandler<GetTodayAttendanceQuery, AttendanceEntity?>
{
    public async Task<AttendanceEntity?> Handle(GetTodayAttendanceQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        return await repository.GetAll(a => a.EmployeeId == request.EmployeeId)
            .Where(a => a.Date == today)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
