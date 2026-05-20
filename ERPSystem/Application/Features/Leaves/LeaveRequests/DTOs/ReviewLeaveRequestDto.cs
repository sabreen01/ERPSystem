using ERPSystem.Domain.Enums;

namespace ERPSystem.Application.Features.Leaves.LeaveRequests.DTOs;

public record ReviewLeaveRequestDto(
    Guid LeaveRequestId,
    LeaveRequestStatus Status,
    string? RejectionReason,
    Guid ReviewedBy
);
