using DataAcces.Entities;

namespace Services.Dtos.Output;

public class SurveyResponseOutputDto
{
    public int Id { get; set; }
    public int SurveyId { get; set; }

    //public IEnumerable<SurveyAskResponseDto> SurveyAskResponseDto { get; set; }
    public IEnumerable<SurveyAskResponseOutputDto> SurveyAskResponseDto { get; set; }
}

public static class SurveyResponseExtention
{
    public static SurveyResponseOutputDto ToSurveyResponseDto(this SurveyResponse surveyResponse)
    {
        return new SurveyResponseOutputDto()
        {
            Id = surveyResponse.Id,
            SurveyId = surveyResponse.SurveyId,
            SurveyAskResponseDto = surveyResponse.SurveyAskResponses!.Select(x =>
                x.ToSurveyOutputtDto()
            )
        };
    }
}
