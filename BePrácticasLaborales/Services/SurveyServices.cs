using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Dtos;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BePrácticasLaborales.Services;

public class SurveyServices : CustomServiceBase
{
    public SurveyServices(EntityDbContext context) : base(context)
    {
    }
    
    public OneOf<ResponseErrorDto, ICollection<Survey>> OrganizatinoWithMoreSurvey()
    {
        var surveys = _context.Surveys.Include(x => x.SurveyAsks)
            .ThenInclude(x => x.SurveyResponses)
            .OrderByDescending(x => x.SurveyAsks.Sum(z => z.SurveyResponses!.Count())).ToList();
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }
        return surveys;
    }
    public async Task<OneOf<ResponseErrorDto, ICollection<Survey>>> SurveyByOrganizatino(int organizationId)
    {
        var organization = await _context.Organization.SingleOrDefaultAsync(x=>x.Id == organizationId);
        if (organization is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization not found"

            };
        }
        var surveys = new List<Survey>();
        if (organization is Ministery)
        {
            surveys = _context.Surveys.ToList();
            if (!surveys.Any())
            {
                return new ResponseErrorDto()
                {
                    ErrorCode = 404,
                    ErrorMessage = $"Survey list of organization with id {organizationId} not found"
                };
                
            }
        }
        else
        {
            surveys = _context.Surveys.Where(x => x.OrganizationId == organizationId).ToList();
            if (!surveys.Any())
            {
                return new ResponseErrorDto()
                {
                    ErrorCode = 404,
                    ErrorMessage = $"Survey list of organization with id {organizationId} not found"
                };
                
            }
        }
        
        return surveys;
    }
    public OneOf<ResponseErrorDto, ICollection<SurveyAsk>> SurveyAskBysurvey(int surveyId)
    {
        var surveys = new List<SurveyAsk>();
        surveys = _context.SurveyAsks.Where(x => x.SurveyId == surveyId).ToList();
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey ask list of survey with id {surveyId} not found"
            };
                
        }
        return surveys;
    }
    public OneOf<ResponseErrorDto, ICollection<ResponsePosibility>> SurveyResponsePosibilityBysurvey(int surveyAskId)
    {
        var responsePosibilities = new List<ResponsePosibility>();
        responsePosibilities = _context.ResponsePosibilities.Where(x => x.SuveryAskId == surveyAskId).ToList();
        if (!responsePosibilities.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Responose posibility list of survey ask with id {surveyAskId} not found"
            };
                
        }
        return responsePosibilities;
    }
    public OneOf<ResponseErrorDto, int> SurveyResponseBysurveyask(int surveyAskId, int reponse)
    {
        var cant =
            _context.SurveyResponses.Count(x => x.SuveryAskId == surveyAskId && x.ResponsePosibilityId == reponse);
        if (cant ==0)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"not one response with reponse {reponse} for survey ask with id {surveyAskId}"
            };
                
        }
        return cant;
    }  
    
    
}