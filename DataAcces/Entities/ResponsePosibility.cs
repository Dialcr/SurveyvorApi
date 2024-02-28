using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BePrácticasLaborales.DataAcces;

namespace DataAcces.Entities;

public class ResponsePosibility
{
    
    [Key]
    [Column(Order = 0)]
    public int Id { get; set; }
    [Key]
    [Column(Order = 1)]
    public int SuveryAskId { get; set; }

    [ForeignKey(nameof(SuveryAskId))]
    public SurveyAsk? SurveyAsk { get; set; }

    [Required]
    [MaxLength(50)]
    public string ResponseValue{ get; set; }

    public ICollection<SurveyResponse>? SurveyResponses { get; set; }
    
    
    
}