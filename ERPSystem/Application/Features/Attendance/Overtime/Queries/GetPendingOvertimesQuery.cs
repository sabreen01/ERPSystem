using ERPSystem.Application.Features.Attendance.Overtime.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;

namespace ERPSystem.Application.Features.Attendance.Overtime.Queries;

public record GetPendingOvertimesQuery() : IRequest<RequestResult<List<PendingOvertimeDto>>>;

public class GetPendingOvertimesQueryHandler(IRepository<AttendanceEntity> repository) 
    : IRequestHandler<GetPendingOvertimesQuery, RequestResult<List<PendingOvertimeDto>>>
{
    public async Task<RequestResult<List<PendingOvertimeDto>>> Handle(GetPendingOvertimesQuery request, CancellationToken cancellationToken)
    {
        var pendingRecords = await repository.GetAll(a => a.OvertimeStatus == OvertimeStatus.Pending)
            .Include(a => a.Employee)
            .Select(a => new PendingOvertimeDto(
                a.Id,
                a.EmployeeId,
                a.Employee != null ? $"{a.Employee.FirstName} {a.Employee.LastName}" : "Unknown",
                a.Date,
                a.CheckInTime,
                a.CheckOutTime,
                a.OvertimeHours ?? 0
            ))
            .ToListAsync(cancellationToken);

        return RequestResult<List<PendingOvertimeDto>>.Success(pendingRecords);
    }
}
