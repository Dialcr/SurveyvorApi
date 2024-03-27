namespace Services.Dtos.Input;

public class SurveyAskInputDto
{
    public string Question { get; set; }
    //public IEnumerable<ResponsePosibilityInputDto>? Answers { get; set; }
    public IEnumerable<string>? Answers { get; set; }
}