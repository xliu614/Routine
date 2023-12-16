using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.DtoParems
{
    public class EmployeeDtoParameters
    {
        private const int MaxPageSize = 20;
        [FromQuery(Name="Gender")]
        public string? GenderDisplay { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;

        public int PageSize {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string OrderBy { get; set; } = "Name";
    }
}
