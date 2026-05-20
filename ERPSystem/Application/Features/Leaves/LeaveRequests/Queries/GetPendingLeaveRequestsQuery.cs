using ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Enums;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.Queries;

public record GetPendingLeaveRequestsQuery : IRequest<RequestResult<List<LeaveRequestResponseDto>>>;

public class GetPendingLeaveRequestsQueryHandler(IRepository<LeaveRequest> repository)
    : IRequestHandler<GetPendingLeaveRequestsQuery, RequestResult<List<LeaveRequestResponseDto>>>
{
    public async Task<RequestResult<List<LeaveRequestResponseDto>>> Handle(GetPendingLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        var pendingRequests = await repository
            .GetAll(lr => lr.Status == LeaveRequestStatus.Pending)
            .Select(lr => new LeaveRequestResponseDto(
                lr.Id,
                lr.EmployeeId,
                lr.LeaveTypeId,
                lr.StartDate,
                lr.EndDate,
                lr.DaysCount,
                lr.Reason,
                lr.Status,
                lr.LeaveRequestDays.Select(d => d.Date).ToList(),
                lr.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        return RequestResult<List<LeaveRequestResponseDto>>.Success(pendingRequests) ;
    }
}
