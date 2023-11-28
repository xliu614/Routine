using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper, ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId) {
            if (!await _companyRepository.CompanyExistsAsync(companyId)) {
                return NotFound();
            }
            var employees = await _companyRepository.GetEmployeesAsync(companyId);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeesDto);
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _companyRepository.GetEmployeeAsync(companyId, employeeId);  
            
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDto>(_mapper.Map<EmployeeDto>(employee)));
        }

    }
}
