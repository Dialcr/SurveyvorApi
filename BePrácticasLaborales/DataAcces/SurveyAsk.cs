using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BePrácticasLaborales.DataAcces;

public class SurveyAsk
{
    [Key]
    public int Id { get; set; }
    
    [Key]
    public int SurveyId { get; set; }
    [ForeignKey(nameof(SurveyId))]
    public Survey Survey { get; set; }
    
    // todo: posibles respuestas para las preguntas  
}