using DataAcces.Entities;

namespace Services.Dtos;

public class SurveyOutputDto
{
    
    public int Id { get; set; }

    public string Description { get; set; } = "";

    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }= "";
    public IEnumerable<SurveyAskOutputDto> SurveyAskOutputDtos{ get; set; }
    
    public int SurveyParticipants { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

}
public static class SurveyExtention
{
    public static SurveyOutputDto ToSurveyOutputDtoWithResponses(this Survey survey)
    {
        return new SurveyOutputDto()
        {
            Id = survey.Id,
            Description = survey.Description,
            OrganizationId = survey.OrganizationId,
            OrganizationName = survey.Organization!.Name,
            SurveyAskOutputDtos = survey.SurveyAsks!.Select(x=>x.ToSurveyAskOutputDtoWithResponses()),
            StartDate = survey.StartDate,
            EndDate = survey.EndDate,
            
                //todo: si es pregunta de texto no tiene funciona la cuenta de la siguite linea
            //todo cambiar eso 
            SurveyParticipants = survey.SurveyAsks!.ToList()[0].SurveyResponses!.Count(),
            
        };

    }
    public static SurveyOutputDto ToSurveyOutputDto(this Survey survey)
    {
        return new SurveyOutputDto()
        {
            Id = survey.Id,
            Description = survey.Description,
            OrganizationId = survey.OrganizationId,
            OrganizationName = survey.Organization!.Name,
            SurveyAskOutputDtos = survey.SurveyAsks!.Select(x=>x.ToSurveyAskOutputDtoWithResponses()),
            StartDate = survey.StartDate,
            EndDate = survey.EndDate,
        };

    }
}