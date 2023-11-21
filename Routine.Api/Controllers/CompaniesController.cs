using Microsoft.AspNetCore.Mvc;
using Routine.Api.Services;

namespace Routine.Api.Controllers
{
    //if the controller is used for generating visualized web pages, then should think of inheritance from Controller class
    //Controller class is also a sub class from ControllerBase
    [ApiController]
    public class CompaniesController:ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies() {
            var companies = await _companyRepository.GetCompaniesAsync();
            return new JsonResult(companies);
        }
    }
}
