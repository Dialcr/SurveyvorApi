namespace Services.Dtos.Input;

public class SurveyResponsesDto
{
    public int SurveyId { get; set; }
    public IEnumerable<ResponseDto> Responses { get; set; }
}