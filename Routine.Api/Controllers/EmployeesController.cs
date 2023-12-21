using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
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
        [HttpGet(Name =(nameof(GetEmployeesForCompany)))]
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

        [HttpPost(Name=(nameof(CreateEmployeeForCompany)))]
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
            if (employeeEntity == null) {
                //return NotFound();
                var addEntity = _mapper.Map<Employee>(employee);
                addEntity.Id = employeeId;
                addEntity.CompanyId = companyId;
                _companyRepository.AddEmployee(companyId, addEntity);
                await _companyRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<EmployeeDto>(addEntity);
                return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId = companyId, employeeId = dtoToReturn.Id }, dtoToReturn);
            }
            //convert employeeEnity to EmployeeDto since the source is employee of the type EmployeeDto
            //employee's property or data now update EmployeeDto
            //then convert the EmployeeDto back to Employee or employeeEntity in this case
            _mapper.Map(employee, employeeEntity);
            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartialUpdateEmployee(Guid companyId, Guid employeeId, JsonPatchDocument<EmployeeUpdateDto> patchDocument) {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
                return NotFound();
            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            
            if (employeeEntity == null) {
               var employeeDto = new EmployeeUpdateDto();
               patchDocument.ApplyTo(employeeDto, ModelState);

                if (!TryValidateModel(employeeDto))
                    return ValidationProblem(ModelState);

                var employeeToAdd = _mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;

                _companyRepository.AddEmployee(companyId, employeeToAdd);
                await _companyRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<EmployeeDto>(employeeToAdd);
                return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, employeeId = employeeId }, dtoToReturn);
            }
                
            var patchEntity = _mapper.Map<EmployeeUpdateDto>(employeeEntity);
            patchDocument.ApplyTo(patchEntity, ModelState);
            
            //validate if there's error in patchEntity
            if (!TryValidateModel(patchEntity))
                return ValidationProblem(ModelState);
            _mapper.Map(patchEntity, employeeEntity);
            _companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId) {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
                return NotFound();

            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);

            if (employeeEntity == null)
                return NotFound();

            _companyRepository.DeleteEmployee(employeeEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
