using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities;

public class Survey
{
    //todo: si la encuesta tiene alguna pregunta sin posibles respuestas asociadas, es una pregunta que tiene respuesta de texto 
    [Key]
    public int Id { get; set; }

    [MaxLength(25)]
    public string Tittle { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    
    //todo: hacer trigger para cambiar el esatdo de aviable si ha sido rechazada la solicitud asociada
    public bool Available { get; set; }
    public int OrganizationId { get; set; }
    
    [ForeignKey(nameof(OrganizationId))]
    public University? Organization { get; set; }

    public Application? Application { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    [Required]
    public IEnumerable<SurveyAsk>? SurveyAsks { get; set; }
}