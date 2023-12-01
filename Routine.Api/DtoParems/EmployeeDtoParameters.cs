using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.DtoParems
{
    public class EmployeeDtoParameters
    {
        [FromQuery(Name="Gender")]
        public string? GenderDisplay { get; set; }
        public string? SearchTerm { get; set; }
    }
}
