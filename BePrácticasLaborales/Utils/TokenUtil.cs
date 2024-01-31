using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BePrácticasLaborales.Utils;

public class TokenUtil
{

    readonly UserManager<IdentityUser<int>> _userManager;
    readonly JwtSettings _jwtSettings;

    public TokenUtil(UserManager<IdentityUser<int>> userManager, IOptions<JwtSettings> jwtsettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtsettings.Value;
    }

    public async Task<string> GenerateTokenAsync(IdentityUser<int> user)
    {
        string? role = null;

        var userRoles = await _userManager.GetRolesAsync(user) ?? new List<string>();

        //todo: cambiar los roles 
        if (userRoles.ToList().Count == 0)
            role = RoleNames.Customer;
        else
            role = userRoles.ToList().First().ToUpper();
        

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, $"{user.Id}"),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Sid, $"{user.Id}"),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role ?? RoleNames.Customer),
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}