using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataAcces.Entities;

public class User : IdentityUser<int>
{
    [MaxLength(100)]
    public string Image { get; set; } =
        "http://gravatar.com/avatar/${md5(this.username)}?d=identicon";
    public int? OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public University? Organization { get; set; }
}
