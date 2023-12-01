using Routine.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.ValidationAttributes
{
    public class EmployeeNoMustDifferentFromNameAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var addDto = (EmployeeAddOrUpdateDto) validationContext.ObjectInstance;
            if (addDto.EmployeeNo == addDto.FirstName || addDto.EmployeeNo == addDto.LastName)
                return new ValidationResult(ErrorMessage, new[] { nameof(EmployeeAddOrUpdateDto) });          
            return ValidationResult.Success;
        }
    }
}
