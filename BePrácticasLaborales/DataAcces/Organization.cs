using System.ComponentModel.DataAnnotations;

namespace BePrácticasLaborales.DataAcces;

public class Organization
{
    [Key]
    public int  Id { get; set; }
    
    [MaxLength(25)]
    public string Name { get; set; }

    public bool Enable { get; set; }
    public List<User>? Users { get; set; }
}