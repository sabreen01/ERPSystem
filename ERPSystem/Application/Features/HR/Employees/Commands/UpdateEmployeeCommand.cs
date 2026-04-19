using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Employees.Commands;

public record UpdateEmployeeCommand(Guid Id, UpdateEmployeeDto Data) : IRequest<bool>;

public class UpdateEmployeeCommandHandler(IRepository<Employee> repository) 
    : IRequestHandler<UpdateEmployeeCommand, bool>
{
    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetById(request.Id);
        if (employee == null) return false;

        employee.FirstName = request.Data.FirstName;
        employee.LastName = request.Data.LastName;
        employee.DateOfBirth = DateTime.SpecifyKind(request.Data.DateOfBirth, DateTimeKind.Utc);
        employee.Gender = request.Data.Gender;
        employee.NationalId = request.Data.NationalId;
        employee.PassportNo = request.Data.PassportNo;
        employee.Nationality = request.Data.Nationality;
        employee.PersonalEmail = request.Data.PersonalEmail;
        employee.PersonalPhone = request.Data.PersonalPhone;
        employee.Address = request.Data.Address;
        employee.HireDate = DateTime.SpecifyKind(request.Data.HireDate, DateTimeKind.Utc);
        employee.ContractType = request.Data.ContractType;
        employee.BranchId = request.Data.BranchId;
        employee.DepartmentId = request.Data.DepartmentId;
        employee.PositionId = request.Data.PositionId;
        employee.SalaryStructureId = request.Data.SalaryStructureId;
        employee.BasicSalary = request.Data.BasicSalary;
        employee.BankName = request.Data.BankName;
        employee.BankAccountNo = request.Data.BankAccountNo;
        employee.BankIban = request.Data.BankIban;
        employee.IsActive = request.Data.IsActive;

        repository.Update(employee);
        await repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
