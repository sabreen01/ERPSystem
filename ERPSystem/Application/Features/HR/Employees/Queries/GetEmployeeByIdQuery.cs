using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Employees.Queries;

public record GetEmployeeByIdQuery(Guid Id) : IRequest<RequestResult<EmployeeDetailsResponseDto>>;

public class GetEmployeeByIdQueryHandler(IRepository<Employee> repository) 
    : IRequestHandler<GetEmployeeByIdQuery, RequestResult<EmployeeDetailsResponseDto>>
{
    public async Task<RequestResult<EmployeeDetailsResponseDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var e = await repository.GetAll(x => x.Id == request.Id)
            .Include(x => x.Position)
            .Include(x => x.Department)
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(cancellationToken);

        if (e == null)
        {
            return RequestResult<EmployeeDetailsResponseDto>.Failure("Employee not found.");
        }

        var dto = new EmployeeDetailsResponseDto(
            e.Id,
            e.EmployeeNo,
            e.FirstName,
            e.LastName,
            e.DateOfBirth,
            e.Gender,
            e.NationalId,
            e.Nationality,
            e.PersonalEmail,
            e.PersonalPhone,
            e.HireDate,
            e.ContractType,
            e.PositionId,
            e.Position?.Name,
            e.DepartmentId,
            e.Department?.Name,
            e.BranchId,
            e.Branch?.Name,
            e.BasicSalary,
            e.IsActive
        );

        return RequestResult<EmployeeDetailsResponseDto>.Success(dto);
    }
}
