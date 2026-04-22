using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using AttendanceEntity = ERPSystem.Domain.Entities.AttendanceManagment.Attendance;

namespace ERPSystem.Application.Features.Attendance.Overtime.Commands;

public record ReviewOvertimeCommand(Guid AttendanceId, OvertimeStatus NewStatus) : IRequest<RequestResult<bool>>;

public class ReviewOvertimeCommandHandler(IRepository<AttendanceEntity> repository) 
    : IRequestHandler<ReviewOvertimeCommand, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(ReviewOvertimeCommand request, CancellationToken cancellationToken)
    {
       
        if (request.NewStatus != OvertimeStatus.Approved && request.NewStatus != OvertimeStatus.Rejected)
        {
            return RequestResult<bool>.Failure("Status can only be set to Approved or Rejected.");
        }

       
        var attendance = await repository.GetById(request.AttendanceId);
        
        if (attendance == null)
        {
            return RequestResult<bool>.Failure("Attendance record not found.");
        }

        if (attendance.OvertimeStatus != OvertimeStatus.Pending)
        {
            return RequestResult<bool>.Failure($"Cannot review this record. Current status is {attendance.OvertimeStatus}.");
        }

       
        attendance.OvertimeStatus = request.NewStatus;
        repository.Update(attendance);
        await repository.SaveChangesAsync(cancellationToken);

        string actionStr = request.NewStatus == OvertimeStatus.Approved ? "approved" : "rejected";
        return RequestResult<bool>.Success(true, $"Overtime successfully {actionStr}.");
    }
}
