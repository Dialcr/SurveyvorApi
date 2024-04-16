using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;

namespace DataAcces.Entities;

public class Application
{
    [Key]
    public int Id { get; set; }

    public ApplicationState ApplicationState { get; set; }

    public int SurveyId { get; set; }

    [ForeignKey(nameof(SurveyId))]
    public Survey? Survey { get; set; }
}
