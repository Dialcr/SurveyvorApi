using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using Microsoft.AspNetCore.Identity;

namespace Services.Dtos;

public class UserOutputDto
{
    
    public string UserName { get; set; } = "";
        
    public string Email { get; set; } = "";

    public string Role { get; set; } = "";
}
