using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Services.Dtos;

namespace Services.Services;

public class SurveyServices : CustomServiceBase
{
    public SurveyServices(EntityDbContext context) : base(context)
    {
    }
    
    public OneOf<ResponseErrorDto, ICollection<SurveyoutputDto>> OrganizatinoWithMoreSurvey()
    {
        var surveys = _context.Surveys
            .Include(x=>x.SurveyAsks)
            .Include(x=>x.Organization)
            .OrderByDescending(x => x.SurveyAsks.Sum(z => z.SurveyResponses!.Count())).ToList();
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }
        return surveys.Select(x=> x.ToSurveyOutputtDto()).ToList();
    }
    public async Task<OneOf<ResponseErrorDto, ICollection<SurveyoutputDto>>> SurveyByUniversity(int universityId)
    {
        var organization = await _context.University.SingleOrDefaultAsync(x=>x.Id == universityId);
        if (organization is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization not found"

            };
        }
        var surveys = new List<Survey>();
        surveys = _context.Surveys.Include(x=>x.Organization).Where(x => x.OrganizationId == universityId).ToList();
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey list of organization with id {universityId} not found"
            };
                
        }
        
        return surveys.Select(x=>x.ToSurveyOutputtDto()).ToList();
    }
    public OneOf<ResponseErrorDto, ICollection<SurveyAsk>> SurveyAskBySurveyId(int surveyId)
    {
        var surveys = _context.SurveyAsks.Where(x => x.SurveyId == surveyId).ToList();
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
        var responsePosibilities = _context.ResponsePosibilities
            .Where(x => x.SuveryAskId == surveyAskId).ToList();
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
    public OneOf<ResponseErrorDto, int> CountSurveyResponsesBysurveyask(int surveyAskId, int reponse)
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
    public OneOf<ResponseErrorDto, double> PorcentSurveyResponsesBySurveyask(int surveyAskId, int reponse)
    {
        var cant =
            _context.SurveyResponses.Count(x => x.SuveryAskId == surveyAskId && x.ResponsePosibilityId == reponse);
        var total = _context.SurveyResponses.Count(x => x.SuveryAskId == surveyAskId);
        if (cant ==0)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"not one response with reponse {reponse} for survey ask with id {surveyAskId}"
            };
                
        }

        return (cant* 100.0) / total;
    }
    public OneOf<ResponseErrorDto, double> AllSurvey(int surveyAskId, int reponse)
    {
        var cant =
            _context.SurveyResponses.Count(x => x.SuveryAskId == surveyAskId && x.ResponsePosibilityId == reponse);
        var total = _context.SurveyResponses.Count();
        if (cant ==0)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"not one response with reponse {reponse} for survey ask with id {surveyAskId}"
            };
                
        }

        return (cant* 100.0) / total;
    }

    public OneOf<ResponseErrorDto, ICollection<SurveyResponseOutputDto>> GetAllResponseBySurveyAskDescription(int surveyId, string surveyAskDescription)
    {
        var surveyResponses =
            _context.SurveyResponses.Include(x=>x.ResponsePosibility)
                .Include(x=>x.SurveyAsk)
                .Where(x => x.SurveyAsk!.SurveyId== surveyId 
                            && x.SurveyAsk.Description == surveyAskDescription);
       
        if (!surveyResponses.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"not one response from survey question {surveyAskDescription} for survey with id {surveyId}"
            };
                
        }

        return surveyResponses.Select(x=>x.ToSurveyOutputtDto()).ToList();
    }
    public OneOf<ResponseErrorDto, double> PorcentSurveyResponsesBySurveyaskDescription(int surveyId,string surveyAskDescription, string reponse)
    {
        var cant =
            _context.SurveyResponses.Count(x => x.SurveyAsk!.SurveyId ==  surveyId
                                                && x.SurveyAsk.Description == surveyAskDescription
                                                && x.ResponsePosibility!.ResponseValue == reponse);
        var total = _context.SurveyResponses.Count(x => x.SurveyAsk!.SurveyId ==  surveyId
                                                        && x.SurveyAsk.Description == surveyAskDescription);
        if (cant ==0)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage =
                    $"not one response with reponse {reponse} for survey ask {surveyAskDescription} from survey {surveyId}"
            };
        }

        return (cant* 100.0) / total;
    }
    public async Task<OneOf<ResponseErrorDto, IEnumerable<SurveyoutputDto>>> SurveyByUniversityName(string universityName)
    {
        var organization = await _context.University.SingleOrDefaultAsync(x=>x.Name == universityName);
        if (organization is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization not found"

            };
        }
        var surveys = new List<Survey>();
        surveys = _context.Surveys.Include(x=>x.Organization).Where(x => x.Organization!.Name == universityName).ToList();
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey list of organization {universityName} not found"
            };
                
        }
        return surveys.Select(x=>x.ToSurveyOutputtDto()).ToList();
    }
}