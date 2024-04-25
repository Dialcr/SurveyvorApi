namespace Services.Dtos.Output;

public class SurveyAskResponseDto
{
    public int SurveyAskId { get; set; }
    public string SurveyAskValue { get; set; } = "";
    public ResponsePosibilityDto ResponsePosibilityDto { get; set; }
}
