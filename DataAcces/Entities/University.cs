using System.ComponentModel.DataAnnotations;

namespace DataAcces.Entities;

public class University 
{
    [Key]
    public int  Id { get; set; }
    
    [MaxLength(25)]
    public string Name { get; set; }
    
    [MaxLength(25)]
    public string Email { get; set; }

    public bool Enable { get; set; }
    
    public string Description { get; set; }
    
    //imagenes son 2 en string base64
    public string ProfileImage { get; set; } 
    
    public string BgImage { get; set; }
    
    public int FacultiesNumber { get; set; }
    public ICollection<User>? Users { get; set; }

    public ICollection<Survey> Surveys { get; set; }   
}