using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;

namespace Services.Dtos;

public class ApplicationOutputDto
{
    public int Id { get; set; }

    public SurveyOutputDto Survey { get; set; }

    public ApplicationState ApplicationState { get; set; }
}

public static class ApplicationExtention
{
    public static ApplicationOutputDto ToApplicationOutputDto(this Application application)
    {
        return new ApplicationOutputDto()
        {
            Id = application.Id,
            ApplicationState = application.ApplicationState,
            Survey = application.Survey!.ToSurveyOutputDto()
        };
    }
}
