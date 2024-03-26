using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

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
    
    //todo: agragar imagenes son 2
    public byte[] ProfileImage { get; set; } = File.ReadAllBytes("./../DataAcces/Images/university1.jpg");
    
    public byte[] BgImage { get; set; } = File.ReadAllBytes("./../DataAcces/Images/university2.jpg");
    
    public int FacultiesNumber { get; set; }
    public ICollection<User>? Users { get; set; }

    public ICollection<Survey> Surveys { get; set; }   
}