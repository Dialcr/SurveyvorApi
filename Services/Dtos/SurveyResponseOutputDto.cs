using DataAcces.Entities;

namespace Services.Dtos;

public class SurveyResponseOutputDto
{
    public int Id { get; set; }
    
    public int ResponsePosibilityId { get; set; }
    public string ResponsePosibility { get; set; }
    public int SuveryAskId { get; set; }
    public string SurveyAsk { get; set; }
}

public static class SurveyResponseExtention
{
    public static SurveyResponseOutputDto ToSurveyOutputtDto(this SurveyResponse surveyResponse)
    {
        return new SurveyResponseOutputDto()
        {
            Id = surveyResponse.Id,
            ResponsePosibility = surveyResponse.ResponsePosibility!.ResponseValue,
            ResponsePosibilityId = surveyResponse.ResponsePosibilityId,
            SurveyAsk = surveyResponse.SurveyAsk!.Description,
            SuveryAskId = surveyResponse.SuveryAskId
        };

    }
}