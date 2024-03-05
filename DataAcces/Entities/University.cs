using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities;

public class University : Organization
{
    public int  MinisteryId { get; set; }
    
    [ForeignKey(nameof(MinisteryId))]
    public Ministery?  Ministery{ get; set; }
}