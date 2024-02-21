using System.ComponentModel.DataAnnotations;

namespace BePrácticasLaborales.DataAcces;

public class Organization
{
    [Key]
    public int  Id { get; set; }
    
    [MaxLength(25)]
    public string Name { get; set; }

    public bool Enable { get; set; }
    public ICollection<User>? Users { get; set; }

    public ICollection<Survey> Surveys { get; set; }
}