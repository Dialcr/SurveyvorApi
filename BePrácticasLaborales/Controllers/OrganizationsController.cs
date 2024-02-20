using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Dtos;
using BePrácticasLaborales.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf;

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
    public async Task<IActionResult> GetUniversityById(int miinsteryId)
    {
        var result = await _organizationServices.GetUniversity(miinsteryId);
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
    [ProducesResponseType(typeof(Organization), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("editOrganization")]
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