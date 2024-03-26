using Services.Dtos.Input;

namespace Services.Dtos.Intput;

public class SurveyInputDto
{
    public string Tittle { get; set; } = "";
    public string Description { get; set; } = "";
    public int OrganizationId { get; set; } = 0;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public ICollection<SurveyAskInputDto>? SurveyAsks { get; set; }
}