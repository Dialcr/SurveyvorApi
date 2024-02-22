using System.ComponentModel.DataAnnotations;

namespace BePrácticasLaborales.DataAcces;

public class SurveyResponse
{
    [Key]
    public int Id { get; set; }
    [Key]
    public int SuveryAskId { get; set; }

    public string value { get; set; }

    public ICollection<ResponsePosibility> ResponsePosibilities { get; set; }
}