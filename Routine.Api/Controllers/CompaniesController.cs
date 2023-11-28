using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;

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
        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        /// <summary>
        /// Get companies list
        /// If you know about the specific return type, then use ActionResult is better than IActionResult
        /// Also, if using ActionResult, then you can return either Ok wrapped result or direct result
        /// </summary>
        /// <returns></returns>
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies() {
            var companies = await _companyRepository.GetCompaniesAsync();
            var companyDtos = new List<CompanyDto>();
            foreach (var company in companies) {
                companyDtos.Add(new CompanyDto
                {
                    Id = company.Id,
                    Name = company.Name,
                });
            }
            return companyDtos;
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompanyById(Guid companyId) //api/companies/123
        {
            //if doing judgement for exist like this, then after exist is true if someone remove the resource then there's a risk of actual not founding the content
            //var exist = await _companyRepository.CompanyExistsAsync(companyId);
            //if (!exist)
            //    return NotFound();

            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
                return NotFound();

            return Ok(company);
        }


    }
}
