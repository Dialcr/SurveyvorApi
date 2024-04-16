using DataAcces.Entities;
using Services.Dtos.Input;

namespace Services.Dtos.Output;

public class SurveyAskOutputDto
{
    public int Id { get; set; }

    public string Description { get; set; } = "";
    public IEnumerable<ResponsePosibilityDto>? ResponsePosibilitys { get; set; }
}

public static class SurveyAskExtention
{
    public static SurveyAskOutputDto ToSurveyAskOutputDtoWithResponsesPosibilities(
        this SurveyAsk surveyAsk
    )
    {
        return new SurveyAskOutputDto()
        {
            Id = surveyAsk.Id,
            Description = surveyAsk.Description,
            ResponsePosibilitys = surveyAsk.ResponsePosibilities!.Select(x =>
                x.ToResponsePosibilityDto()
            )
        };
    }

    public static SurveyAskOutputDto ToSurveyAskOutputDtoWithOutResponsesPosibilities(
        this SurveyAsk surveyAsk
    )
    {
        return new SurveyAskOutputDto() { Id = surveyAsk.Id, Description = surveyAsk.Description, };
    }
}
