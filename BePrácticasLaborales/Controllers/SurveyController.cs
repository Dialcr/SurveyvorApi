using BePrácticasLaborales.Middleware;
using DataAcces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using OneOf;
using Services.Dtos;
using Services.Dtos.Input;
using Services.Dtos.Intput;
using Services.Dtos.Output;
using Services.Utils;

namespace BePrácticasLaborales.Controllers;

[ApiController]
[Route("[Controller]")]
public class SurveyController(SurveyServices surveyServices, ImportDbServices importDbServices, 
    TokenUtil tokenUtil ,OrganizationServices organizationServices) : ControllerBase
{
    
    [HttpPost]
    [Route("/import/data")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ImportInfo()
    { 
        //var info =await _importDbServices.ImportData(organizationId);  
        var info =await importDbServices.FindObject();
        if (info.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/OrganizationWithMoreSurvey")]
    [ProducesResponseType(typeof(ICollection<UniversityOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN")]
    public IActionResult OrganizationWithMoreSurvey()
    {
        var result = surveyServices.OrganizationWithMoreSurvey();
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpGet]
    [Route("/OrganizationWithMoreSurveyResponses")]
    [ProducesResponseType(typeof(ICollection<UniversityOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN")]
    public IActionResult OrganizationWithMoreSurveyResponses()
    {
        var result = surveyServices.OrganizationWithMoreSurveyResponses();
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    
    [HttpGet]
    [Route("/SurveyWithMoreResponses")]
    [ProducesResponseType(typeof(ICollection<SurveyOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> SurveyWithMoreResponses()
    {
        string? accessToken = HttpContext
            .Request.Headers["Authorization"]
            .FirstOrDefault()
            ?.Split(" ")
            .Last();
        accessToken = accessToken!.Replace("Bearer", "");
        var userId = tokenUtil.GetUserIdFromToken(accessToken);
        var organization = await organizationServices.GetUniversityByUserAsync(userId);
        var result = surveyServices.SurveyWithMoreResponses(organization.AsT1.Id);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpGet]
    [Route("/SurveyByUniversity")]
    [ProducesResponseType(typeof(ICollection<SurveyOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> SurveyByUniversity()
    { 
        string? accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        accessToken = accessToken!.Replace("Bearer", "");
        var userId = tokenUtil.GetUserIdFromToken(accessToken);
        var organization = await organizationServices.GetUniversityByUserAsync(userId);
        //var result = await surveyServices.SurveyByUniversityId(organization.AsT1.Id);
        var result = await surveyServices.SurveyActiveByUniversityId(organization.AsT1.Id);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }

        return Ok(response);
    }
    [HttpGet]
    [Route("/SurveyActiveByUniversity")]
    [ProducesResponseType(typeof(ICollection<SurveyOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> SurveyActiveByUniversity(int organizationId)
    { 
        
        var result = await surveyServices.SurveyActiveByUniversityId(organizationId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }

        return Ok(response);
    }
    [HttpGet]
    [Route("/SurveyAskBySurveyId")]
    [ProducesResponseType(typeof(ICollection<SurveyAsk>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public IActionResult SurveyAskBySurveyId(int surveyId )
    { 
        var result =  surveyServices.SurveyAskBySurveyId(surveyId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    
    [HttpGet]
    [Route("/SurveyResponsePosibilityBysurvey")]
    [ProducesResponseType(typeof(ICollection<ResponsePosibility>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public  IActionResult SurveyResponsePosibilityBysurvey(int surveyAskId )
    { 
        var result =  surveyServices.SurveyResponsePosibilityBySurvey(surveyAskId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/CountSurveyResponsesBysurveyask")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public  IActionResult CountSurveyResponsesBysurveyask(int surveyAskId , int responseId)
    { 
        var result =  surveyServices.CountSurveyResponsesBysurveyask(surveyAskId, responseId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/PorcentSurveyResponsesBysurveyask")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public  IActionResult PorcentSurveyResponsesBysurveyask(int surveyAskId , int responseId)
    { 
        var result =  surveyServices.PorcentSurveyResponsesBySurveyask(surveyAskId, responseId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/PorcentSurveyResponsesBySurveyaskDescription")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public  IActionResult PorcentSurveyResponsesBySurveyaskDescription(int surveyId,string surveyAskDescription, string reponse)
    { 
        var result =  surveyServices.PorcentSurveyResponsesBySurveyaskDescription(surveyId,surveyAskDescription, reponse);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/GetAllResponseBySurveyAskDescription")]
    [ProducesResponseType(typeof(ICollection<SurveyAskResponseOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public  IActionResult GetAllResponseBySurveyAskDescription(int surveyId, string surveyAskDescription)
    { 
        var result =  surveyServices.GetAllResponseBySurveyAskDescription(surveyId,surveyAskDescription);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/SurveyByUniversityName")]
    [ProducesResponseType(typeof(ICollection<SurveyOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    [CheckSurveyDate]
    public async Task<IActionResult> SurveyByUniversityName(string organizationName)
    { 
        var result =  await surveyServices.SurveyByUniversityName(organizationName);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/GetSurveyInfo")]
    [ProducesResponseType(typeof(SurveyOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    [CheckSurveyDate]
    public IActionResult GetSurveyInfo(int surveyId)
    { 
        var result =  surveyServices.GetSurveyInfo(surveyId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    
    [HttpPost]
    [Route("/ApplicateSurveyAsync")]
    [ProducesResponseType(typeof(SurveyOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> ApplicateSurveyAsync(SurveyInputDto surveyInput)
    { 
        string? accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        accessToken = accessToken!.Replace("Bearer", "");
        var userId = tokenUtil.GetUserIdFromToken(accessToken);
        var organization = await organizationServices.GetUniversityByUserAsync(userId);
        surveyInput.OrganizationId = organization.AsT1.Id;
        var result =  await surveyServices.ApplicateSurveyAsync(surveyInput);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpGet]
    [Route("/GetAllApplicationsSurveys")]
    [ProducesResponseType(typeof(IEnumerable<ApplicationOutputDto>), StatusCodes.Status200OK)]
    [Authorize(Roles = "ADMIN")]
    public IActionResult GetAllApplicationsSurveys()
    { 
        var result =  surveyServices.GetAllApplicationsSurveys();
        return Ok(result);
    } 
    
    [HttpGet]
    [Route("/GetApplicationsSurveyInfo")]
    [ProducesResponseType(typeof(ApplicationOutputDto), StatusCodes.Status200OK)]
    [Authorize(Roles = "ADMIN")]
    public IActionResult GetApplicationsSurveyInfo(int applicationId)
    { 
        var result =  surveyServices.GetApplicationsSurveyInfo(applicationId);
        return Ok(result);
    } 
    [HttpPatch]
    [Route("/RespondApplicationsSurvey")]
    [ProducesResponseType(typeof(ApplicationOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> RespondApplicationsSurvey(RespondApplicationDto respondApplicationDto)
    { 
        var result =  await surveyServices.RespondApplicationsSurveyAsync(respondApplicationDto.ApplicationId, 
            respondApplicationDto.Accepted);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
    [HttpPatch]
    [Route("/DisableSurvey")]
    [ProducesResponseType(typeof(SurveyOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DisableSurvey([FromBody]int surveyId)
    { 
        var result =  await surveyServices.DisableSurveyAsync(surveyId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    } 
    [HttpGet]
    [Route("/GetAllPendingApplicationsSurveys")]
    [ProducesResponseType(typeof(IEnumerable<ApplicationOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN")]
    public IActionResult GetAllPendingApplicationsSurveys()
    { 
        var result =  surveyServices.GetAllPendingApplicationsSurveys();
        return Ok(result);
    }
    [HttpGet]
    [Route("/GetAllRejectedApplicationsSurveys")]
    [ProducesResponseType(typeof(IEnumerable<ApplicationOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> GetAllRejectedApplicationsSurveys()
    { 
        string? accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        accessToken = accessToken!.Replace("Bearer", "");
        var userId = tokenUtil.GetUserIdFromToken(accessToken);
        var organization = await organizationServices.GetUniversityByUserAsync(userId);
        if (organization.TryPickT0(out var error, out var university))
        {
            return NotFound(error);
        }
        var result =  surveyServices.GetAllRejectedApplicationsSurveys(university.Id);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("/AddResponseToSurvey")]
    [ProducesResponseType(typeof(SurveyResponsesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> AddResponseToSurvey([FromBody]SurveyResponsesDto surveyResponsesDto)
    { 
        var resutl = await surveyServices.AddResponseToSurvey(surveyResponsesDto);
        if (resutl.TryPickT0(out var error, out var responses))
        {
            return BadRequest(error);
        }
        return Ok(responses);
    }
    [HttpGet]
    [Route("/GetResponseFromSurvey")]
    [AllowAnonymous]
    public IActionResult GetResponseFromSurvey(int surveyId)
    { 
        var resutl = surveyServices.GetResponseFromSurvey(surveyId);
        if (!resutl.Any())
        {
            return BadRequest(resutl);
        }
        return Ok(resutl);
    }
    
}