using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;

namespace DataAcces.Entities;

public class ResponsePosibility
{
    
    [Key]
    public int Id { get; set; }
    
    public int? SuveryAskId { get; set; }

    [ForeignKey(nameof(SuveryAskId))]
    public SurveyAsk? SurveyAsk { get; set; }

    [Required]
    [MaxLength(50)]
    public string ResponseValue{ get; set; }

    public ICollection<SurveyResponse>? SurveyResponses { get; set; }
    
    
    
}