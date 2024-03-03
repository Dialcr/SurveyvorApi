
using DataAcces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Services.Dtos;
using Services.Services;

namespace BePrácticasLaborales.Controllers;

public class OrganizationsController : ControllerBase
{
    private readonly OrganizationServices _organizationServices;

    public OrganizationsController(OrganizationServices organizationServices)
    {
        _organizationServices = organizationServices;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Ministery), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
    [Route("getMinisteryById")]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> GetMinisteryById(int ministeryId)
    {
        var result = await _organizationServices.GetMinistery(ministeryId);
        if (result.TryPickT0(out var error, out var response))
        {
            return NotFound(error);
        }
        return Ok(response);
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
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
    [Route("getAllUniversiti")]
    [Authorize(Roles = "ORGANIZATION")]
    public IActionResult GetAllUniversity(int miinsteryId)
    {
        var result = _organizationServices.GetAllUniversity(miinsteryId);
        if (result.TryPickT0(out var error, out var response))
        {
            return NotFound(error);
        }
        return Ok(response);
    }
    [HttpPost]
    [ProducesResponseType(typeof(University), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("addUniversity")]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> AddUniversity(UniversitiIntputDto universitiIntputDto)
    {
        var result = await _organizationServices.Adduniversity(universitiIntputDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpPost]
    [ProducesResponseType(typeof(Ministery), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("addMinistery")]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> AddMinistery(UniversitiIntputDto ministery)
    {
        var result = await _organizationServices.AddMinnistery(ministery);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpPut]
    [ProducesResponseType(typeof(Organization), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("editOrganization")]
    [Authorize(Roles = "ORGANIZATION")] 
    public async Task<IActionResult> EditOrganization(OrganizatinosIntupDto organizatinosIntupDto, int organizationId)
    {
        var result = await _organizationServices.EditOrganization(organizationId,organizatinosIntupDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    
    
    
}