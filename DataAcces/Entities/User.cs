using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataAcces.Entities;

public class User : IdentityUser<int>
{
    public byte[] Image { get; set; } = File.ReadAllBytes("./../DataAcces/Images/user.jpg");
    public int? OrganizationId { get; set; }
    
    [ForeignKey(nameof(OrganizationId))]
    public University? Organization { get; set; }
    
}