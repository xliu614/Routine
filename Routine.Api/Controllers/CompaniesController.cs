using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Routine.Api.DtoParems;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Routine.Api.Controllers
{
    //if the controller is used for generating visualized web pages, then should think of inheritance from Controller class
    //Controller class is also a sub class from ControllerBase
    [ApiController]
    [Route("api/companies")]
    //[Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper, 
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        }
        /// <summary>
        /// Get companies list
        /// If you know about the specific return type, then use ActionResult is better than IActionResult
        /// Also, if using ActionResult, then you can return either Ok wrapped result or direct result
        /// </summary>
        /// <returns>If choose HttpHead then the there's no body in the response</returns>
        [HttpGet(Name=nameof(GetCompanies))]
        [HttpHead]
        public async Task<IActionResult> GetCompanies([FromQuery]CompanyDtoParameters? parameters) {

            if (!_propertyMappingService.ValidMappingExistsFor<CompanyDto, Company>(parameters.OrderBy)) {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(parameters.Fields)) {
                return BadRequest();
            }
            var companies = await _companyRepository.GetCompaniesAsync(parameters);
            var previousPageLink = companies.HasPrevious? CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = companies.HasNext ? CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage) : null;

            //anonymous type
            var paginationMetadata = new
            {
                totalCount = companies.Count,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions()
            {
              Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            //The returned values can be not a complete CompanyDto
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            
            return Ok(companyDtos.ShapeData(parameters.Fields));
        }

        [HttpGet("{companyId}", Name = nameof(GetCompanyById))]
        public async Task<IActionResult> GetCompanyById(Guid companyId, string? fields) //api/companies/123
        {
            //if doing judgement for exist like this, then after exist is true if someone remove the resource then there's a risk of actual not founding the content
            //var exist = await _companyRepository.CompanyExistsAsync(companyId);
            //if (!exist)
            //    return NotFound();

            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(fields))
            {
                return BadRequest();
            }

            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
                return NotFound();

            var links = CreateLinksForCompany(companyId, fields);

            var linksDict = _mapper.Map<CompanyDto>(company).ShapeData(fields) as IDictionary<string, object>;

            linksDict.Add("links", links);

            return Ok(linksDict);
        }

        /// <summary>
        /// this method allows for adding a company and if there are related employees of the company, they can be added as well
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyAddDto company) {
            //ApiController already did this, so this is not in need
            //if (company == null)
            //    return BadRequest();
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);

            var links = CreateLinksForCompany(returnDto.Id, null);
            var linksDict = returnDto.ShapeData(null) as IDictionary<string, object>;

            linksDict.Add("links", links);

            return CreatedAtRoute(nameof(GetCompanyById), new { companyId = returnDto.Id }, linksDict);
        }

        /// <summary>
        /// GetCompanyCollection where the input are a collection of companyIds
        /// Use ArrayModelBinder to convert a string to an array of Guid
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("~/api/GetCompanyCollection/({ids})", Name =nameof(GetCompanyCollection))]      
        public async Task<IActionResult> GetCompanyCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> ids) 
        {
            if (ids == null) {
                return BadRequest();
            }
            var entities = await _companyRepository.GetCompaniesAsync(ids);

            if (ids.Count() != entities.Count())
                return NotFound();

            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtosToReturn);
        }

        [HttpPost]
        [Route("~/api/companies-multi")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateMultipleCompanies([FromBody] IEnumerable<CompanyAddDto> companyCollection)
        {
            var entities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var entity in entities) {
                _companyRepository.AddCompany(entity);
            }            
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<IEnumerable<CompanyDto>>(entities);

            var ids = string.Join(",", returnDto.Select(r => r.Id));

            return CreatedAtRoute(nameof(GetCompanyCollection),new { ids = ids }, returnDto);
        }
        [HttpOptions]
        public IActionResult GetCompaniesOptions() {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }
        [HttpDelete("{companyId}", Name =(nameof(DeleteCompany)))]
        public async Task<IActionResult> DeleteCompany(Guid companyId) {
            var companyEntity = await _companyRepository.GetCompanyAsync(companyId);
            if (companyEntity == null)
                return NotFound();
            //Load all the company's employees to memory
            await _companyRepository.GetEmployeesAsync(companyId, null);
            _companyRepository.DeleteCompany(companyEntity);
            await _companyRepository.SaveAsync();
            return NoContent();
        }

        private string CreateCompaniesResourceUri(CompanyDtoParameters parameters, ResourceUriType type) {
            switch (type) {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    }) ;

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });

                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });
            }

        }

        private IEnumerable<LinkDto> CreateLinksForCompany(Guid companyId, string fields) {
            var links = new List<LinkDto>();
            //Add link for Getthe company by Id (and fields)
            if (fields == null)
            {
                links.Add(new LinkDto(Url.Link(nameof(GetCompanyById), new { companyId }),
                          "self", "GET"));
            }
            else {
                links.Add(new LinkDto(Url.Link(nameof(GetCompanyById), new { companyId, fields }),
                           "self", "GET"));
            }

            //Add link for Remove the company by Id
            links.Add(new LinkDto(Url.Link(nameof(DeleteCompany), new { companyId}),
                           "delete_company", "DELETE"));

            //Add link for Creating employees for the company
            links.Add(new LinkDto(Url.Link(nameof(EmployeesController.CreateEmployeeForCompany), new { companyId }),
                           "create_employee_for_company", "POST"));

            //Add link for get employees for the company
            links.Add(new LinkDto(Url.Link(nameof(EmployeesController.GetEmployeesForCompany), new { companyId }),
                           "get_employees_for_company", "GET"));
            return links;
        }
    }
}
