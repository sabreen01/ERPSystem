using System.ComponentModel.DataAnnotations.Schema;
using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Enums;

namespace ERPSystem.Domain.Entities.HR;
[Table(name : "Employee" , Schema = "HR")]
public class Employee: BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string NationalId { get; set; } = string.Empty; 
    public string? PassportNo { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string? PersonalEmail { get; set; }
    public string PersonalPhone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? PhotoPath { get; set; }
    public string EmployeeNo { get; set; } = string.Empty; // EMP-YYYY-NNNN
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;
    public Gender Gender { get; set; }
    public ContractType ContractType { get; set; }
    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }
    public Guid PositionId { get; set; }
    public Position? Position { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid SalaryStructureId { get; set; }
    public decimal BasicSalary { get; set; }
    public string? BankName { get; set; }
    public string? BankAccountNo { get; set; }
    public string? BankIban { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
}