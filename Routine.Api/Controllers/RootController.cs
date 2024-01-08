﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;

namespace Routine.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot() {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(Url.Link(nameof(GetRoot), new { }), "self", "GET"));
            links.Add(new LinkDto(Url.Link(nameof(CompaniesController.GetCompanies),new { }), "companies", "GET"));
            links.Add(new LinkDto(Url.Link(nameof(CompaniesController.CreateCompany), new { }), "crete_company", "POST"));
            return Ok(links);
        }
    }
}
