using Routine.Api.Entities;
using Routine.Api.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Routine.Api.Models
{
    /// <summary>
    /// EmployeAddDto may have some customized validtion, so it inherits from IValidatableObject
    /// Create a parent class for EmployeeAddDto which is also parent for EmployeeUpdateDto
    /// </summary>
    [EmployeeNoMustDifferentFromNameAttribute(ErrorMessage = "Employee No has to be diffrent with the Name!")]
    public class EmployeeAddOrUpdateDto : IValidatableObject
    {
        [Display(Name = "EmployeeNo")]
        [Required(ErrorMessage = "You have to input {0}")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0}'s length is {1}")]
        public string? EmployeeNo { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You have to input {0}")]
        [MaxLength(50, ErrorMessage = "{0}'s length cannot be larger than {1}")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name"), Required(ErrorMessage = "You have to input {0}"), MaxLength(50, ErrorMessage = "{0}'s length cannot be larger than {1}")]
        public string? LastName { get; set; }
        
        [Range(1,2,ErrorMessage="Invalid gender value, value must be 1 or 2")]
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        //this is often about class level or some validtions after the attributes validation already passed
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
                yield return new ValidationResult("Last Name cannot be the same as First Name",
                    new[] { nameof(FirstName), nameof(LastName) });
        }
    }
}
