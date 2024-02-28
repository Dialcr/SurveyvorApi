using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;

namespace DataAcces.Entities;

public class SurveyAsk
{
    [Key]
    public int Id { get; set; }
    
    public int SurveyId { get; set; }
    
    [ForeignKey(nameof(SurveyId))]
    public Survey Survey { get; set; }

    [Required]
    [MaxLength(150)]
    public string Description { get; set; }
    
    public ICollection<ResponsePosibility>? ResponsePosibilities { get; set; }
    
    public ICollection<SurveyResponse>? SurveyResponses { get; set; }
    
}