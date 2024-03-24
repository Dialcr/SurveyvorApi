using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Services.Dtos;

public class UserOutputDto
{
    
    public string Name { get; set; } = "";
        
    public string Email { get; set; } = "";

    public string Role { get; set; } = "";

    public byte[] Image { get; set; } = File.ReadAllBytes("./../DataAcces/Images/user.jpg");
}


public static class UserExtention
{
    public static UserOutputDto ToUserOutputDto(this User user)
    {
        return new UserOutputDto()
        {
            Name = user.UserName!,
            Email = user.Email!,
            Image = user.Image,
        };

    }
}
