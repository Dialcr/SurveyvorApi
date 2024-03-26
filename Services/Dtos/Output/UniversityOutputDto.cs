using DataAcces.Entities;

namespace Services.Dtos;

public class UniversityOutputDto
{
    public int Id { get; set; }
    public string Name { get; set; } ="";
    
    public string Email { get; set; }="";
    public bool Enable { get; set; }
    public string Description { get; set; }="";
    
    //todo: agragar imagenes son 2
    public string? ProfileImage { get; set; } = Convert.ToBase64String(File.ReadAllBytes("./../DataAcces/Images/university1.jpg"));
    
    public string? BgImage { get; set; } = Convert.ToBase64String(File.ReadAllBytes("./../DataAcces/Images/university2.jpg"));
    
    public int FacultiesNumber { get; set; }
}


public static class UniversityExtention
{
    public static UniversityOutputDto ToUniversityOutputDto(this University university)
    {
        return new UniversityOutputDto()
        {
            Id = university.Id,
            Enable = university.Enable,
            Name = university.Name,
            FacultiesNumber = university.FacultiesNumber,
            Email = university.Email,
            Description = university.Description,
            ProfileImage = university.ProfileImage,
            BgImage = university.BgImage,
            
        };

    }
}

