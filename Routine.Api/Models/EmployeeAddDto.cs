using Routine.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    /// <summary>
    /// EmployeAddDto may have some customized validtion, so it inherits from IValidatableObject
    /// </summary>
    public class EmployeeAddDto : IValidatableObject
    {
        [Display(Name = "EmployeeNo")]
        [Required(ErrorMessage="You have to input {0}")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "{0}'s length is {1}")]
        public string? EmployeeNo { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You have to input {0}")]
        [MaxLength(50, ErrorMessage = "{0}'s length cannot be larger than {1}")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name"), Required(ErrorMessage = "You have to input {0}"), MaxLength(50, ErrorMessage = "{0}'s length cannot be larger than {1}")]
        public string? LastName { get; set; }
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
