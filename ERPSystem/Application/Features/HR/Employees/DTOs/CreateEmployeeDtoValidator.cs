using FluentValidation;

namespace ERPSystem.Application.Features.HR.Employees.DTOs;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

        RuleFor(x => x.PersonalEmail)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.PersonalEmail))
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.PersonalPhone)
            .NotEmpty().WithMessage("Personal Phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("A valid phone number is required.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .Length(14).WithMessage("National ID must be exactly 14 characters.");

        RuleFor(x => x.BasicSalary)
            .GreaterThan(0).WithMessage("Basic Salary must be greater than 0.");
            
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");
            
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
            
        RuleFor(x => x.PositionId)
            .NotEmpty().WithMessage("Position ID is required.");
    }
}
