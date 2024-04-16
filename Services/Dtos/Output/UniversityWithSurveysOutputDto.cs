namespace Services.Dtos.Output;

public class UniversityWithSurveysOutputDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public string Email { get; set; } = "";
    public bool Enable { get; set; }
    public string Description { get; set; } = "";

    public string? ProfileImage { get; set; } =
        Convert.ToBase64String(File.ReadAllBytes("./../DataAcces/Images/university1.jpg"));

    public string? BgImage { get; set; } =
        Convert.ToBase64String(File.ReadAllBytes("./../DataAcces/Images/university2.jpg"));

    public int FacultiesNumber { get; set; }

    public int SurveyAviableCount { get; set; }
}
