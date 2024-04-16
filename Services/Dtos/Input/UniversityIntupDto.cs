namespace Services.Dtos;

public class UniversityIntupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public bool Enable { get; set; } = true;

    public string Email { get; set; } = "";

    public string Description { get; set; } = "";

    public string? ProfileImage { get; set; } = "";

    public string? BgImage { get; set; } = "";

    public int FacultiesNumber { get; set; } = 1;
}
