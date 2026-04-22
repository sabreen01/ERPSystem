using ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;

public record GetAllLeaveTypesQuery() : IRequest<RequestResult<List<LeaveTypeResponseDto>>>;

public class GetAllLeaveTypesQueryHandler(IRepository<LeaveType> repository)
    : IRequestHandler<GetAllLeaveTypesQuery, RequestResult<List<LeaveTypeResponseDto>>>
{
    public async Task<RequestResult<List<LeaveTypeResponseDto>>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        var leaveTypes = await repository.GetAll()
            .Select(lt => new LeaveTypeResponseDto(
                lt.Id,
                lt.Name,
                lt.Code,
                lt.DaysPerYear,
                lt.IsPaid,
                lt.CarryForward,
                lt.MaxCarryDays,
                lt.RequiresApproval,
                lt.MinNoticeDays,
                lt.IsActive
            ))
            .ToListAsync(cancellationToken);

        return RequestResult<List<LeaveTypeResponseDto>>.Success(leaveTypes);
    }
}
