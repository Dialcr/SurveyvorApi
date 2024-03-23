using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAcces.Entities;

public class University 
{
    [Key]
    public int  Id { get; set; }
    
    [MaxLength(25)]
    public string Name { get; set; }

    public bool Enable { get; set; }
    
    //todo: agregar descripcion
    public string Description { get; set; }
    
    //todo: agragar imagenes son 2
    public ICollection<User>? Users { get; set; }

    public ICollection<Survey> Surveys { get; set; }   
}