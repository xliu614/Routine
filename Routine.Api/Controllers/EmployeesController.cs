using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParems;
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
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId, [FromQuery]EmployeeDtoParameters? parameters) {
            if (!await _companyRepository.CompanyExistsAsync(companyId)) {
                return NotFound();
            }
            var employees = await _companyRepository.GetEmployeesAsync(companyId, parameters);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeesDto);
        }

        [HttpGet("{employeeId}", Name=nameof(GetEmployeeForCompany))]
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

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee) {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
                return NotFound();
            var entity = _mapper.Map<Employee>(employee);
            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<EmployeeDto>(entity);
            return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId = companyId, employeeId = dtoToReturn.Id }, dtoToReturn);
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeUpdateDto employee) {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
                return NotFound();
            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null) {  return NotFound(); }
            //convert employeeEnity to EmployeeDto since the source is employee of the type EmployeeDto
            //employee's property or data now update EmployeeDto
            //then convert the EmployeeDto back to Employee or employeeEntity in this case
            _mapper.Map(employee, employeeEntity);
            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
    }
}
