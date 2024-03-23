using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;

namespace DataAcces.Entities;

//encuesta
public class Survey
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; }

    public string SatiscationState { get; set; }

    public int OrganizationId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public University? Organization { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public ICollection<SurveyAsk> SurveyAsks { get; set; }
}