using System.ComponentModel.DataAnnotations.Schema;

namespace BePrácticasLaborales.DataAcces;

public class University : Organization
{
    //todo: duda pueden haber varios ministerios?
    public int  MinisteryId { get; set; }
    
    [ForeignKey(nameof(MinisteryId))]
    public Ministery?  Ministery{ get; set; }
}