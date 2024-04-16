using DataAcces.Entities;

namespace Services.Dtos.Output;

public class OrganizationOutputDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public static class UniversityExtention
{
    public static OrganizationOutputDto ToOrganizationOutputDto(this University university)
    {
        return new OrganizationOutputDto() { Id = university.Id, Name = university.Name };
    }
}
