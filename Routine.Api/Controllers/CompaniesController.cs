using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParems;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;
using System.Collections.Generic;

namespace Routine.Api.Controllers
{
    //if the controller is used for generating visualized web pages, then should think of inheritance from Controller class
    //Controller class is also a sub class from ControllerBase
    [ApiController]
    [Route("api/companies")]
    //[Route("api/[controller]")]
    public class CompaniesController:ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Get companies list
        /// If you know about the specific return type, then use ActionResult is better than IActionResult
        /// Also, if using ActionResult, then you can return either Ok wrapped result or direct result
        /// </summary>
        /// <returns>If choose HttpHead then the there's no body in the response</returns>
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters? parameters) {

            var companies = await _companyRepository.GetCompaniesAsync(parameters);
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            
            return Ok(companyDtos);
        }

        [HttpGet("{companyId}", Name = nameof(GetCompanyById))]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid companyId) //api/companies/123
        {
            //if doing judgement for exist like this, then after exist is true if someone remove the resource then there's a risk of actual not founding the content
            //var exist = await _companyRepository.CompanyExistsAsync(companyId);
            //if (!exist)
            //    return NotFound();

            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
                return NotFound();

            return Ok(_mapper.Map<CompanyDto>(company));
        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyAddDto company) {
            //ApiController already did this, so this is not in need
            //if (company == null)
            //    return BadRequest();
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);
            return CreatedAtRoute(nameof(GetCompanyById), new { companyId = returnDto.Id }, returnDto);
        }

    }
}
