namespace Services.Dtos;

public class UserIntputDto
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public int? OrganizationId { get; set; }
    public string? Image { get; set; } = "";


}
