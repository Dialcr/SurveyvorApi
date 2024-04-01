using DataAcces.Entities;

namespace Services.Dtos.Output;

public class SurveyAskOutputDto
{
    public int Id { get; set; } 
    
    public string Description { get; set; } = "";
}

public static class SurveyAskExtention
{
    public static SurveyAskOutputDto ToSurveyAskOutputDtoWithResponses(this SurveyAsk surveyAsk)
    {
        return new SurveyAskOutputDto()
        {
            Id = surveyAsk.Id,
            Description = surveyAsk.Description,
        };
    }
}