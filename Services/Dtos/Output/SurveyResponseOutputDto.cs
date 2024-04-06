using DataAcces.Entities;

namespace Services.Dtos;

public class SurveyResponseOutputDto
{
    public int ResponsePosibilityId { get; set; }
    public string ResponsePosibility { get; set; }
    public int SuveryAskId { get; set; }
    public string SurveyAsk { get; set; }
}

public static class SurveyResponseExtention
{
    public static SurveyResponseOutputDto ToSurveyOutputtDto(this SurveyAskResponse surveyAskResponse)
    {
        return new SurveyResponseOutputDto()
        {
            ResponsePosibility = surveyAskResponse.ResponsePosibility!.ResponseValue,
            ResponsePosibilityId = surveyAskResponse.ResponsePosibilityId,
            SurveyAsk = surveyAskResponse.SurveyAsk!.Description,
            SuveryAskId = surveyAskResponse.SuveryAskId
        };

    }
}