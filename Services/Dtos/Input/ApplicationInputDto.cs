using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Services.Dtos.Intput;

namespace Services.Dtos;

public class ApplicationInputDto
{
    public SurveyInputDto Survey { get; set; }
}
