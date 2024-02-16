using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BePrácticasLaborales.DataAcces;

public class User : IdentityUser<int>
{
    public int? OrganizationId { get; set; }
    
    [ForeignKey(nameof(OrganizationId))]
    public Organization? Organization { get; set; }
    
}