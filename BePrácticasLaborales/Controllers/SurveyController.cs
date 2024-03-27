using DataAcces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using OneOf;
using Services.Dtos;
using Services.Dtos.Intput;
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
    [Route("/OrganizatinoWithMoreSurvey")]
    [ProducesResponseType(typeof(ICollection<SurveyOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    //todo: arreglar este metodo
    public IActionResult OrganizatinoWithMoreSurvey(int ministeryId )
    { 
        var result = surveyServices.OrganizatinoWithMoreSurvey();
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
    public async Task<IActionResult> SurveyByUniversity(int organizationId )
    { 
        var result = await surveyServices.SurveyByUniversityId(organizationId);
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
    [ProducesResponseType(typeof(ICollection<SurveyResponseOutputDto>), StatusCodes.Status200OK)]
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
    [Authorize(Roles = "ORGANIZATION")]
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
        //todo: arreglar el terror este 
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
    
}