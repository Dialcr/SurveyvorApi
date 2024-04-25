using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities;

public class SurveyResponse
{
    [Key]
    public int Id { get; set; }

    public int SurveyId { get; set; }

    [ForeignKey(nameof(SurveyId))]
    public Survey? Survey { get; set; }

    public IEnumerable<SurveyAskResponse>? SurveyAskResponses { get; set; }
}
