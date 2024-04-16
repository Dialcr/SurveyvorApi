using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Services.Dtos;

public class UserOutputDto
{
    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string Role { get; set; } = "";

    public string Image { get; set; } =
        "http://gravatar.com/avatar/${md5(this.username)}?d=identicon";
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
