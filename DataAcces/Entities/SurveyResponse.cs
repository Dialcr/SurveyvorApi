using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities;

public class SurveyResponse
{
    //[Column(Order = 0)]
    [Key]
    public int Id { get; set; }

    /*
    [Key]
    [Column(Order = 1)]
    public int SuveryAskId { get; set; }

    [ForeignKey(nameof(SuveryAskId))]
    public SurveyAsk? SurveyAsk { get; set; }
    */

    public int ResponsePosibilityId { get; set; }

    [ForeignKey(nameof(ResponsePosibilityId))]
    public ResponsePosibility? ResponsePosibility { get; set; }
    public int SuveryAskId { get; set; }

    [ForeignKey(nameof(SuveryAskId))]
    public SurveyAsk? SurveyAsk { get; set; }
}
