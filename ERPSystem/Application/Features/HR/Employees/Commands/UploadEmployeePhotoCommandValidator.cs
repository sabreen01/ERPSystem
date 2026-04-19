using FluentValidation;

namespace ERPSystem.Application.Features.HR.Employees.Commands;

public class UploadEmployeePhotoCommandValidator : AbstractValidator<UploadEmployeePhotoCommand>
{
    public UploadEmployeePhotoCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.Photo)
            .NotNull().WithMessage("Photo is required.")
            .Must(file => file.Length > 0).WithMessage("Photo cannot be empty.")
            .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("Photo must be less than 5 MB.")
            .Must(file => 
            {
                var ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                return ext == ".jpg" || ext == ".jpeg" || ext == ".png";
            }).WithMessage("Only JPG and PNG files are allowed.");
    }
}
