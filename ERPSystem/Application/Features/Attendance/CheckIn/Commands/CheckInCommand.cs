using ERPSystem.Domain.Interfaces;
using MediatR;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;

namespace ERPSystem.Application.Features.Attendance.CheckIn.Commands;

public record CheckInCommand(Guid EmployeeId, ERPSystem.Domain.Enums.AttendanceStatus Status) : IRequest<Guid>;

public class CheckInCommandHandler(IRepository<AttendanceEntity> repository) 
    : IRequestHandler<CheckInCommand, Guid>
{
    public async Task<Guid> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        var attendance = new AttendanceEntity
        {
            EmployeeId = request.EmployeeId,
            Date = DateTime.UtcNow.Date,
            CheckInTime = DateTime.UtcNow,
            Status = request.Status
        };

        repository.Add(attendance);
        await repository.SaveChangesAsync(cancellationToken);

        return attendance.Id;
    }
}
