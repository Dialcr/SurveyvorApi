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
    [HttpGet]
    [Route("/import/data")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Testenpoint(string token, string userName, int organizationId )
    { 
        var info =await _importDbServices.ImportData(organizationId);  
        return Ok(info);
        
    }
    [HttpGet]
    [Route("/OrganizatinoWithMoreSurvey")]
    [ProducesResponseType(typeof(ICollection<Survey>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    //todo: arreglar este metodo
    public OneOf<ResponseErrorDto, IActionResult> OrganizatinoWithMoreSurvey(int ministeryId )
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
    [ProducesResponseType(typeof(ICollection<Survey>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<OneOf<ResponseErrorDto, IActionResult>> SurveyByUniversity(int organizationId )
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
    public OneOf<ResponseErrorDto, IActionResult> SurveyAskBySurveyId(int surveyId )
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
    public OneOf<ResponseErrorDto, IActionResult> SurveyResponsePosibilityBysurvey(int surveyAskId )
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
    public OneOf<ResponseErrorDto, IActionResult> CountSurveyResponsesBysurveyask(int surveyAskId , int responseId)
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
    public OneOf<ResponseErrorDto, IActionResult> PorcentSurveyResponsesBysurveyask(int surveyAskId , int responseId)
    { 
        var result =  _surveyServices.PorcentSurveyResponsesBysurveyask(surveyAskId, responseId);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
        
    }
    
}