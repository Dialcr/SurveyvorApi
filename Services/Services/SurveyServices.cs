using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Services.Dtos;
using Services.Dtos.Input;
using Services.Dtos.Intput;
using Services.Dtos.Output;

namespace Services.Services;

public class SurveyServices(EntityDbContext context) : CustomServiceBase(context)
{
    public OneOf<ResponseErrorDto, ICollection<UniversityOutputDto>> OrganizationWithMoreSurvey()
    {
        var organizations = _context.University
            .Include(x => x.Surveys)
            .OrderByDescending(x => x.Surveys.Count);
            
        if (!organizations.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }

        return organizations.Select(x=>x.ToOrganizationOutputDtoSurveyCount()).Take(5).ToList();
    }
    public OneOf<ResponseErrorDto, ICollection<UniversityOutputDto>> OrganizationWithMoreSurveyResponses()
    {
        var organizations = _context.University
            .Include(x => x.Surveys)
            .ThenInclude(x => x.SurveyResponses).ToList();
        if ( !organizations.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }
            var processed = organizations
            .Select(organization => new
            {
                Organization = organization,
                Percentage = organization.Surveys
                    .SelectMany(survey => survey.SurveyResponses ?? null) 
                    .GroupBy(response => response.SurveyId) 
                    .Select(group => new
                    {
                        SurveyId = group.Key,
                        TotalResponses = group.Count()
                    })
                    .Select(survey => (double)survey.TotalResponses / organization.Surveys.Count) 
                    .DefaultIfEmpty(0) 
                    .Average() 
            })
            .OrderByDescending(item => item.Percentage).ToList();
        

        return processed.Select(x=> new UniversityOutputDto()
        {
            Id = x.Organization.Id,
            Enable = x.Organization.Enable,
            Name = x.Organization.Name,
            FacultiesNumber = x.Organization.FacultiesNumber,
            Email = x.Organization.Email,
            Description = x.Organization.Description,
            ProfileImage = x.Organization.ProfileImage,
            BgImage = x.Organization.BgImage,
            Percentage = x.Percentage
        }).Take(5).ToList();
    }
    public OneOf<ResponseErrorDto, ICollection<SurveyOutputDto>> SurveyWithMoreResponses(int universityId)
    {
        var surveys = _context.Surveys.Include(x => x.SurveyAsks!)
            .ThenInclude(x => x.ResponsePosibilities)
            .Include(x=>x.SurveyResponses)
            .Where(x=>x.OrganizationId == universityId)
            .OrderBy(x => x.SurveyResponses!.Count());
        
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Surveys list not found"
            };
        }

        return surveys.Select(x=>x.ToSurveyOutputDto()).Take(5).ToList();
    }
    
    public async Task<OneOf<ResponseErrorDto, ICollection<SurveyOutputDto>>> SurveyByUniversityId(int universityId)
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

        var surveys = _context.Surveys
            .Include(x => x.Organization)
            //.Include(x => x.SurveyAsks)!
            //.ThenInclude(x=>x.SurveyResponses)
            .Include(x => x.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x=>x.SurveyResponses)
            .Where(x=>x.OrganizationId == universityId);
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey list of organization with id {universityId} not found"
            };
                
        }
        
        return surveys.Select(x=> x.ToSurveyOutputDtoWithResponses()).ToList();
    }
    public async Task<OneOf<ResponseErrorDto, ICollection<SurveyOutputDto>>> SurveyActiveByUniversityId(int universityId)
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

        var surveys = _context.Surveys
            .Include(x => x.Organization)
            //.Include(x => x.SurveyAsks)!
            //.ThenInclude(x=>x.SurveyAskResponses)
            .Include(x => x.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x=>x.SurveyResponses)
            .Where(x=>x.OrganizationId == universityId && x.Available);
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey list of organization with id {universityId} not found"
            };
                
        }
        
        return surveys.Select(x=> x.ToSurveyOutputDtoWithResponses()).ToList();
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
    public OneOf<ResponseErrorDto, ICollection<ResponsePosibility>> SurveyResponsePosibilityBySurvey(int surveyAskId)
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
            _context.SurveyAskResponses.Count(x => x.SuveryAskId == surveyAskId && x.ResponsePosibilityId == reponse);
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
            _context.SurveyAskResponses.Count(x => x.SuveryAskId == surveyAskId && x.ResponsePosibilityId == reponse);
        var total = _context.SurveyAskResponses.Count(x => x.SuveryAskId == surveyAskId);
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
            _context.SurveyAskResponses.Count(x => x.SuveryAskId == surveyAskId && x.ResponsePosibilityId == reponse);
        var total = _context.SurveyAskResponses.Count();
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

    public OneOf<ResponseErrorDto, ICollection<SurveyAskResponseOutputDto>> GetAllResponseBySurveyAskDescription(int surveyId, string surveyAskDescription)
    {
        var surveyResponses =
            _context.SurveyAskResponses.Include(x=>x.ResponsePosibility)
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
            _context.SurveyAskResponses.Count(x => x.SurveyAsk!.SurveyId ==  surveyId 
                                                //todo: no se si esto sea realmente necesario
                                                && x.SurveyAsk.Survey.Available
                                                
                                                && x.SurveyAsk.Description == surveyAskDescription
                                                && x.ResponsePosibility!.ResponseValue == reponse);
        var total = _context.SurveyAskResponses.Count(x => x.SurveyAsk!.SurveyId ==  surveyId
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
    public async Task<OneOf<ResponseErrorDto, ICollection<SurveyOutputDto>>> SurveyByUniversityName(string universityName)
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
        var surveys = _context.Surveys
            .Include(x => x.Organization)
            .Include(x => x.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x=>x.SurveyResponses)
            .Where(x => x.Organization!.Name == universityName && x.Available);
        if (!surveys.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey list of organization {universityName} not found"
            };
                
        }
        return surveys.Select(x=> x.ToSurveyOutputDtoWithResponses()).ToList();
    }

    public OneOf<ResponseErrorDto,SurveyOutputDto> GetSurveyInfo(int surveyId)
    {
        var survey = _context.Surveys
            .Include(x => x.Organization)
            //.Include(x => x.SurveyAsks)!
            //.ThenInclude(x=>x.SurveyAskResponses)
            .Include(x => x.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x=>x.SurveyResponses)
            .SingleOrDefault(x => x.Id == surveyId );
        if (survey is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey with id {surveyId} not found"
            };
                
        }
        return survey.ToSurveyOutputDtoWithResponses();
    }
    
//todo:    17=>solicitar crear una encuesta(role organization)
    public async Task<OneOf<ResponseErrorDto, SurveyOutputDto>> ApplicateSurveyAsync(SurveyInputDto surveyInputDto)
    {
        /*
        var survey = new Survey
        {
            Tittle = surveyInputDto.Title,
            Description = surveyInputDto.Description,
            OrganizationId = surveyInputDto.OrganizationId,
            StartDate = surveyInputDto.StartDate,
            EndDate = surveyInputDto.EndDate,
            Available = false,
            SurveyAsks = surveyInputDto.Questions!.Select(x => new SurveyAsk
            {
                Description = x.Question,
                ResponsePosibilities = x.Answers!.Select(y => new ResponsePosibility()
                {
                    ResponseValue = y
                }) ?? []
            })
        };

        var newApplication = new Application()
        {
            ApplicationState = ApplicationState.Pending,
            Survey = survey
        };
        context.Applications.Add(newApplication);
        await _context.SaveChangesAsync();
        return survey.ToSurveyOutputDto();
        */
        
        var survey = new Survey
        {
            Tittle = surveyInputDto.Title,
            Description = surveyInputDto.Description,
            OrganizationId = surveyInputDto.OrganizationId,
            StartDate = surveyInputDto.StartDate,
            EndDate = surveyInputDto.EndDate,
            Available = false,
            SurveyAsks = new List<SurveyAsk>() // Inicializa la colección
        };

        var SurveyAsks = new List<SurveyAsk>(); // Inicializa la colección
        // Agrega cada SurveyAsk individualmente
        foreach (var question in surveyInputDto.Questions!)
        {
            var surveyAsk = new SurveyAsk
            {
                Description = question.Question,
                ResponsePosibilities = question.Answers?.Select(answer => new ResponsePosibility
                {
                    ResponseValue = answer
                }).ToList() ?? new List<ResponsePosibility>()
            };
            //survey.SurveyAsks.ToList().Add(surveyAsk);
            SurveyAsks.Add(surveyAsk);
        }

        survey.SurveyAsks = SurveyAsks;
        var newApplication = new Application()
        {
            ApplicationState = ApplicationState.Pending,
            Survey = survey
        };

        context.Applications.Add(newApplication);
        await _context.SaveChangesAsync();
        var surveyOut = context.Surveys
            .Include(x=>x.Organization)
            .Include(x=>x.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .OrderBy(x=>x.Id)
            .LastOrDefault();
        return surveyOut!.ToSurveyOutputDto();
    }
//todo:        18=>mostrar solicitudes de encuensta(role admin)
    public  IEnumerable<ApplicationOutputDto> GetAllApplicationsSurveys()
    {
        var applications = _context.Applications
            .Include(x => x.Survey)
            .Include(x => x.Survey!.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x => x.Survey!.Organization)
            .Select(x => x.ToApplicationOutputDto());
       
        return applications;
    }
    public  ApplicationOutputDto GetApplicationsSurveyInfo(int applicationId)
    {
        var aplicationInfo = context.Applications
            .Include(x => x.Survey)
            .Include(x => x.Survey!.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x => x.Survey!.Organization).
            FirstOrDefault(x => x.Id == applicationId);
        
        return aplicationInfo.ToApplicationOutputDto();
    }
//todo:        19=>aceptar o rechazar solicitudes de encuestas(role admin)
    public async Task<OneOf<ResponseErrorDto, ApplicationOutputDto>> RespondApplicationsSurveyAsync(int applicationId, bool accepted)
    {
        var application = context.Applications
            .Include(x=>x.Survey)
            .ThenInclude(x=>x.SurveyAsks)
            .ThenInclude(x=>x.ResponsePosibilities)
            .Include(x=>x.Survey)
            .ThenInclude(x=>x.Organization)
            .FirstOrDefault(x=>x.Id == applicationId && x.ApplicationState == ApplicationState.Pending);
        if (application is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Application with id {applicationId} not found"
            };
        }
        if (accepted)
        {
            application.ApplicationState = ApplicationState.Approved;
            application.Survey!.Available = true;
        }
        else
        {
            application.ApplicationState = ApplicationState.Rejected;
        }
            
        await context.SaveChangesAsync();
        return application.ToApplicationOutputDto();
    }

    public async Task<OneOf<ResponseErrorDto, SurveyOutputDto>> DisableSurveyAsync(int surveyId)
    {
        var survey = context.Surveys
            .Include(x=>x.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .FirstOrDefault(x => x.Id == surveyId && x.Available);
        if (survey is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey with id {surveyId} not found"
            };
        }
        survey.Available = false;
        await context.SaveChangesAsync();
        return survey.ToSurveyOutputDto();
    }
    
    
    //todo: devolver todas las solicitudes rechazadas para una orfanizacion
    public IEnumerable<ApplicationOutputDto> GetAllRejectedApplicationsSurveys(int organizationId)
    {
        
        var applications = _context.Applications
            .Include(x => x.Survey)
            .Include(x => x.Survey!.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Where(x => x.ApplicationState == ApplicationState.Rejected && x.Survey!.OrganizationId == organizationId)
            .Select(x => x.ToApplicationOutputDto());
       
        return applications;
    }
    //todo: devolver todas las solicitudes son estado definido
    public IEnumerable<ApplicationOutputDto> GetAllPendingApplicationsSurveys()
    {
        var applications = _context.Applications
            .Include(x => x.Survey)
            .ThenInclude(y=>y!.Organization)
            .Include(x => x.Survey!.SurveyAsks)!
            .ThenInclude(x=>x.ResponsePosibilities)
            .Where(x => x.ApplicationState == ApplicationState.Pending)
            .Select(x => x.ToApplicationOutputDto());
       
        return applications;
    }
    
    
    
    
    public async Task<OneOf<ResponseErrorDto, SurveyResponsesDto>> AddResponseToSurvey( SurveyResponsesDto surveyResponsesDto)
    {
        var surveyQuestions = _context.SurveyAsks.Where(x => x.SurveyId == surveyResponsesDto.SurveyId);
        if (!surveyQuestions.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = $"Survey with id {surveyResponsesDto.SurveyId} not found"
            };
        }
        
        if (surveyResponsesDto.Responses.Count() != surveyQuestions.Count())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 400,
                ErrorMessage = "The number of responses does not match the number of survey questions"
            };
        }
        var surveyResponse = new SurveyResponse()
        {
            SurveyId = surveyResponsesDto.SurveyId,
            SurveyAskResponses = new List<SurveyAskResponse>()
        };
        var newResponses = new List<SurveyAskResponse>();
        foreach (var response in surveyResponsesDto.Responses)
        {
            if (response.ResponsePosibilityId is null)
            {
                newResponses.Add(new SurveyAskResponse()
                {
                    SuveryAskId = response.AskId,
                    ResponsePosibility = new ResponsePosibility()
                    {
                        //SuveryAskId  = response.AskId,
                        ResponseValue = response.ResponseValue,
                    },
                    SurveyResponse = surveyResponse
                });
            }
            else
            {
                newResponses.Add(new SurveyAskResponse()
                {
                    SuveryAskId = response.AskId,
                    ResponsePosibilityId = response.ResponsePosibilityId.Value,
                    SurveyResponse = surveyResponse
                      
                });
            }
                 
        }
        
        try
        {
            //surveyResponse.SurveyId = surveyResponse.SurveyId;
            //surveyResponse.SurveyAskResponses = newResponses;
            //await context.SurveyResponses.AddAsync(surveyResponse);
            await context.SurveyAskResponses.AddRangeAsync(newResponses);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 500,
                ErrorMessage = e.Message
            };
        }
        

        return surveyResponsesDto;
    }

    public IEnumerable<SurveyResponseOutputDto> GetResponseFromSurvey(int surveyId)
    {
        var result = context.SurveyResponses!
            .Include(x => x.SurveyAskResponses!)
            .ThenInclude(x => x.SurveyAsk!)
            .Include(x => x.SurveyAskResponses!)
            .ThenInclude(x => x.ResponsePosibility)
            .Where(x => x.SurveyId == surveyId)
            .Select(x => x.ToSurveyResponseDto());

        return result;
    }
    
}