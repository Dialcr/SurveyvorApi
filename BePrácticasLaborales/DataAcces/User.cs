using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BePrácticasLaborales.DataAcces;

public class User : IdentityUser<int>
{

    [Required] [MaxLength(100)] public string FullName { get; set; } = string.Empty;
}