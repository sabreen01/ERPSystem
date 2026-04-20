using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;
namespace ERPSystem.Application.Features.Attendance.CheckOut.Commands;

public record CheckOutCommand(Guid AttendanceId, double? OvertimeHours, OvertimeStatus OvertimeStatus) : IRequest<bool>;

public class CheckOutCommandHandler(IRepository<AttendanceEntity> repository) 
    : IRequestHandler<CheckOutCommand, bool>
{
    public async Task<bool> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        var attendance = await repository.GetById(request.AttendanceId);
        
        if (attendance == null)
            return false;

        attendance.CheckOutTime = DateTime.UtcNow;
        attendance.OvertimeHours = request.OvertimeHours;
        attendance.OvertimeStatus = request.OvertimeStatus;
        
        repository.Update(attendance);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
