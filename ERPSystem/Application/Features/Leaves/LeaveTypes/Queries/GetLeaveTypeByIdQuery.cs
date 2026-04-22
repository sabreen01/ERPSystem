using ERPSystem.Application.Features.Leaves.LeaveTypes.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.LeaveTypes.Queries;

public record GetLeaveTypeByIdQuery(Guid Id) : IRequest<RequestResult<LeaveTypeResponseDto>>;

public class GetLeaveTypeByIdQueryHandler(IRepository<LeaveType> repository)
    : IRequestHandler<GetLeaveTypeByIdQuery, RequestResult<LeaveTypeResponseDto>>
{
    public async Task<RequestResult<LeaveTypeResponseDto>> Handle(GetLeaveTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetById(request.Id);

        if (entity == null)
            return RequestResult<LeaveTypeResponseDto>.Failure("Leave type not found.");

        var dto = new LeaveTypeResponseDto(
            entity.Id,
            entity.Name,
            entity.Code,
            entity.DaysPerYear,
            entity.IsPaid,
            entity.CarryForward,
            entity.MaxCarryDays,
            entity.RequiresApproval,
            entity.MinNoticeDays,
            entity.IsActive
        );

        return RequestResult<LeaveTypeResponseDto>.Success(dto);
    }
}
