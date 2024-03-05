namespace Services.Dtos;

public class SurveyoutputDto
{
    
    public int Id { get; set; }

    public string Description { get; set; }

    public string SatiscationState { get; set; }

    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }

}