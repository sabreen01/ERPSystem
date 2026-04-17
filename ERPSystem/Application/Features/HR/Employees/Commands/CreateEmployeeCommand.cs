using ERPSystem.Application.Features.HR.Employees.DTOs;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Application.Features.HR.Employees.Commands;

public record CreateEmployeeCommand(CreateEmployeeDto Data) : IRequest<Guid>;
public class CreateEmployeeCommandHandler(IRepository<Employee> repository, IMediator _mediator) 
    : IRequestHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken ct)
    {
        var year = DateTime.Now.Year;
        var lastEmpNo = await repository.GetAll(e => e.EmployeeNo.Contains($"-{year}-"))
            .OrderByDescending(e => e.EmployeeNo)
            .Select(e => e.EmployeeNo)
            .FirstOrDefaultAsync(ct);
        int nextSequence = 1;
        if (!string.IsNullOrEmpty(lastEmpNo))
        {
            var parts = lastEmpNo.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out int lastSeq))
            {
                nextSequence = lastSeq + 1;
            }
        }
        string newEmployeeNo = $"EMP-{year}-{nextSequence:D4}";
        var emp = new Employee
        {
            FirstName = request.Data.FirstName,
            LastName = request.Data.LastName,
            DateOfBirth = DateTime.SpecifyKind(request.Data.DateOfBirth, DateTimeKind.Utc),
            Gender = request.Data.Gender,
            NationalId = request.Data.NationalId,
            PassportNo = request.Data.PassportNo,
            Nationality = request.Data.Nationality,
            PersonalEmail = request.Data.PersonalEmail,
            PersonalPhone = request.Data.PersonalPhone,
            Address = request.Data.Address,
            
            EmployeeNo = newEmployeeNo,
            HireDate = DateTime.SpecifyKind(request.Data.HireDate, DateTimeKind.Utc),
            ContractType = request.Data.ContractType,
            
            BranchId = request.Data.BranchId,
            DepartmentId = request.Data.DepartmentId,
            PositionId = request.Data.PositionId,
            SalaryStructureId = request.Data.SalaryStructureId,
            
            BasicSalary = request.Data.BasicSalary,
            BankName = request.Data.BankName,
            BankAccountNo = request.Data.BankAccountNo,
            BankIban = request.Data.BankIban,
            
            IsActive = true
        };
        
        repository.Add(emp);
        await repository.SaveChangesAsync(ct);

        return emp.Id;
    }
}