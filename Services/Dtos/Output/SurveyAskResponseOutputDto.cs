using DataAcces.Entities;

namespace Services.Dtos;

public class SurveyAskResponseOutputDto
{
    public int ResponsePosibilityId { get; set; }
    public string ResponsePosibility { get; set; } = "";
    public int SuveryAskId { get; set; }
    public string SurveyAsk { get; set; } ="";
}

public static class SurveyResponseExtention
{
    public static SurveyAskResponseOutputDto ToSurveyOutputtDto(this SurveyAskResponse surveyAskResponse)
    {
        return new SurveyAskResponseOutputDto()
        {
            ResponsePosibility = surveyAskResponse.ResponsePosibility!.ResponseValue,
            ResponsePosibilityId = surveyAskResponse.ResponsePosibilityId,
            SurveyAsk = surveyAskResponse.SurveyAsk!.Description,
            SuveryAskId = surveyAskResponse.SuveryAskId
        };

    }
}