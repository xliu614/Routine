using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    /// <summary>
    /// Input(Creation Dto)
    /// </summary>
    public class CompanyAddDto
    {
        [Display(Name="Company Name")]
        [Required(ErrorMessage ="You must input {0}"), MaxLength(100)]
        public string? Name { get; set; }
        [Display(Name="Introduction")]
        [StringLength(500,MinimumLength =10,ErrorMessage ="{0} should have length between {2} and {1}")]
        public string? Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();
    }
}
