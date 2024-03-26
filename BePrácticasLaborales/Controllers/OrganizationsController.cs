using DataAcces.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Services.Dtos;
using Services.Services;
using Services.Utils;

namespace BePrácticasLaborales.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizationsController(OrganizationServices organizationServices, TokenUtil tokenUtil) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(UniversityOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
    [Route("GetUniversityById")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetUniversityById(int universityId)
    {
        
        var result = await organizationServices.GetUniversity(universityId);
        if (result.TryPickT0(out var error, out var response))
        {
            return NotFound(error);
        }
        return Ok(response);
    }
    [HttpGet]
    [ProducesResponseType(typeof(UniversityOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
    [Route("GetUniversityByUser")]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> GetUniversityByUser(string token )
    {
        string? accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        accessToken = accessToken!.Replace("Bearer", "");
        var userId = tokenUtil.GetUserIdFromToken(accessToken);
        var result = await organizationServices.GetUniversityByUserAsync(userId);
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
        var result = organizationServices.GetAllUniversity();
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
    public async Task<IActionResult> AddUniversity(UniversityIntupDto universityIntputDto)
    {
        var result = await organizationServices.AddUniversityAsync(universityIntputDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
   
    [HttpPut]
    [ProducesResponseType(typeof(UniversityOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("editOrganization")]
    [Authorize(Roles = "ADMIN")] 
    public async Task<IActionResult> EditUniversity([FromBody]UniversityIntupDto universityIntupDto)
    {
        var result = await organizationServices.EditOrganization(universityIntupDto.Id,universityIntupDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpPatch]
    [ProducesResponseType(typeof(UniversityOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Route("DisableUniversity")]
    [Authorize(Roles = "ADMIN")] 
    public IActionResult DisableUniversity([FromBody]int universityId)
    {
        var result = organizationServices.DisableUniversity(universityId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    
    
    
}