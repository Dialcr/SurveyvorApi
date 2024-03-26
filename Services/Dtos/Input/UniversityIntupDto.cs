namespace Services.Dtos;

public class UniversityIntupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public bool Enable { get; set; } = true;
    
    public string Email { get; set; }= "";
    
    public string Description { get; set; }= "";
    
    public byte[]? ProfileImage { get; set; } = File.ReadAllBytes("./../DataAcces/Images/university1.jpg");
    
    public byte[]? BgImage { get; set; } = File.ReadAllBytes("./../DataAcces/Images/university2.jpg");

    public int FacultiesNumber { get; set; } = 1;
}