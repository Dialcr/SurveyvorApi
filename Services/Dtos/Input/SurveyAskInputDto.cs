namespace Services.Dtos.Input;

public class SurveyAskInputDto
{
    public int SurveyId { get; set; }
    
    public string Description { get; set; }
    
    public IEnumerable<ResponsePosibilityInputDto>? ResponsePosibilities { get; set; }
}