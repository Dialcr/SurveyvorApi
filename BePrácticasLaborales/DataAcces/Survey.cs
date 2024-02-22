using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BePrácticasLaborales.DataAcces;

//encuesta
public class Survey
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; }

    public string SatiscationState { get; set; }

    public int OrganizationId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public Organization Organization { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}