using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    public IEnumerable<ResponsePosibility>? ResponsePosibilities { get; set; }
    
    public IEnumerable<SurveyAskResponse>? SurveyAskResponses { get; set; }
    
}