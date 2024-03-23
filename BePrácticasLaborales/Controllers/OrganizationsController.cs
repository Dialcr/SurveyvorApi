using DataAcces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Services.Dtos;
using Services.Services;

namespace BePrácticasLaborales.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly OrganizationServices _organizationServices;

    public OrganizationsController(OrganizationServices organizationServices)
    {
        _organizationServices = organizationServices;
    }

    
    [HttpGet]
    [ProducesResponseType(typeof(University), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
    [Route("GetUniversityById")]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> GetUniversityById(int universityId)
    {
        
        var result = await _organizationServices.GetUniversity(universityId);
        if (result.TryPickT0(out var error, out var response))
        {
            return NotFound(error);
        }
        return Ok(response);
    }
   
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<University>), StatusCodes.Status200OK)]
    [Route("getAllUniversity")]
    [AllowAnonymous]
    public IActionResult GetAllUniversity()
    {
        var result = _organizationServices.GetAllUniversity();
        if (!result.Any())
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    [HttpPost]
    [ProducesResponseType(typeof(UniversityOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("addUniversity")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> AddUniversity(UniversitiIntputDto universitiIntputDto)
    {
        var result = await _organizationServices.Adduniversity(universitiIntputDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
   
    [HttpPut]
    [ProducesResponseType(typeof(University), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("editOrganization")]
    [Authorize(Roles = "ADMIN")] 
    public async Task<IActionResult> EditUniversity(UniversityIntupDto universityIntupDto, int organizationId)
    {
        var result = await _organizationServices.EditOrganization(organizationId,universityIntupDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    
    
    
}