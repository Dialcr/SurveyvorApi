using DataAcces.Entities;

namespace Services.Dtos;

public class UniversityOutputDto
{
    public string Name { get; set; }
    
    public bool Enable { get; set; }
    //public int MinisteryId { get; set; }
    public int Id { get; set; }
}


public static class UniversityExtention
{
    public static UniversityOutputDto ToUniversityOutputDto(this University university)
    {
        return new UniversityOutputDto()
        {
            Id = university.Id,
            Enable = university.Enable,
            Name = university.Name
            
        };

    }
}

