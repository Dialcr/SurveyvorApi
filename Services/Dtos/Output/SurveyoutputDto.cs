using DataAcces.Entities;

namespace Services.Dtos;

public class SurveyoutputDto
{
    
    public int Id { get; set; }

    public string Description { get; set; } = "";

    public string SatiscationState { get; set; }= "";

    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }= "";
    public IEnumerable<SurveyAskOutputDto> SurveyAskOutputDtos{ get; set; }
    
    public int SurveyParticipants { get; set; }

}
public static class SurveyExtention
{
    public static SurveyoutputDto ToSurveyOutputDtoWithResponses(this Survey survey)
    {
        return new SurveyoutputDto()
        {
            Id = survey.Id,
            Description = survey.Description,
            SatiscationState = survey.SatiscationState,
            OrganizationId = survey.OrganizationId,
            OrganizationName = survey.Organization!.Name,
            SurveyAskOutputDtos = survey.SurveyAsks.Select(x=>x.ToSurveyOutputDtoWithResponses()),
            SurveyParticipants = survey.SurveyAsks.ToList()[0].SurveyResponses!.Count(),
        };

    }
    public static SurveyoutputDto ToSurveyOutputDto(this Survey survey)
    {
        return new SurveyoutputDto()
        {
            Id = survey.Id,
            Description = survey.Description,
            SatiscationState = survey.SatiscationState,
            OrganizationId = survey.OrganizationId,
            OrganizationName = survey.Organization!.Name,
        };

    }
}