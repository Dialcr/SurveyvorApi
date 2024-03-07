using DataAcces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using OneOf;
using Services.Dtos;

namespace BePrácticasLaborales.Controllers;

[ApiController]
[Route("[Controller]")]
public class SurveyController : ControllerBase
{
    private readonly SurveyServices _surveyServices;
    private readonly ImportDbServices _importDbServices;

    public SurveyController(SurveyServices surveyServices, ImportDbServices importDbServices)
    {
        _surveyServices = surveyServices;
        _importDbServices = importDbServices;
    }
    
    [HttpPost]
    [Route("/import/data")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ImportInfo()
    { 
        //var info =await _importDbServices.ImportData(organizationId);  
        var info =await _importDbServices.FindObject();
        if (info.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/OrganizatinoWithMoreSurvey")]
    [ProducesResponseType(typeof(ICollection<SurveyoutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    //todo: arreglar este metodo
    public IActionResult OrganizatinoWithMoreSurvey(int ministeryId )
    { 
        var result = _surveyServices.OrganizatinoWithMoreSurvey();
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/SurveyByUniversity")]
    [ProducesResponseType(typeof(ICollection<SurveyoutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> SurveyByUniversity(int organizationId )
    { 
        var result = await _surveyServices.SurveyByUniversity(organizationId);
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
        var result =  _surveyServices.SurveyAskBySurveyId(surveyId);
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
        var result =  _surveyServices.SurveyResponsePosibilityBysurvey(surveyAskId);
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
        var result =  _surveyServices.CountSurveyResponsesBysurveyask(surveyAskId, responseId);
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
        var result =  _surveyServices.PorcentSurveyResponsesBySurveyask(surveyAskId, responseId);
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
        var result =  _surveyServices.PorcentSurveyResponsesBySurveyaskDescription(surveyId,surveyAskDescription, reponse);
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
        var result =  _surveyServices.GetAllResponseBySurveyAskDescription(surveyId,surveyAskDescription);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    [HttpGet]
    [Route("/SurveyByUniversityName")]
    [ProducesResponseType(typeof(ICollection<SurveyoutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> SurveyByUniversityName(string organizationName)
    { 
        var result =  await _surveyServices.SurveyByUniversityName(organizationName);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    
}