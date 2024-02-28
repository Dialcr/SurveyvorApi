using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataAcces.Entities;

public class User : IdentityUser<int>
{
    public int? OrganizationId { get; set; }
    
    [ForeignKey(nameof(OrganizationId))]
    public Organization? Organization { get; set; }
    
}