using AutoMapper;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Routine.Api.Entities;
using Routine.Api.Models;

namespace Routine.Api.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.GenderDiplay, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)));
            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
        }

        static int CalculateAge(DateTime birthDate) {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            //if birthDate is later than the calculated back dataTime, which means that it's not yet the birthdate for the employee
            if (birthDate > today.AddYears(-age)) {
                age--;
            }
            return age;
        }
    }
}
