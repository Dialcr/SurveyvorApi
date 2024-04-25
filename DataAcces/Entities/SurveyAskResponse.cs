using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities;

public class SurveyAskResponse
{
    public int SurveyResponseId { get; set; }

    [ForeignKey(nameof(SurveyResponseId))]
    public SurveyResponse? SurveyResponse { get; set; }

    public int ResponsePosibilityId { get; set; }

    [ForeignKey(nameof(ResponsePosibilityId))]
    public ResponsePosibility? ResponsePosibility { get; set; }

    public int SuveryAskId { get; set; }

    [ForeignKey(nameof(SuveryAskId))]
    public SurveyAsk? SurveyAsk { get; set; }
}
