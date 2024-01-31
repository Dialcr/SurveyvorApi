using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Dtos;
using BePrácticasLaborales.Utils;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace BePrácticasLaborales.Services;

public class UserServicers
{
    
    private readonly UserManager<IdentityUser<int>> _userManager;
    private readonly SignInManager<IdentityUser<int>> _signInManager;
    private readonly TokenUtil _tokenUtil;

    public UserServicers(UserManager<IdentityUser<int>> userManager, 
        SignInManager<IdentityUser<int>> signInManager, TokenUtil tokenUtil)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenUtil = tokenUtil;
        
    }

    public async Task<OneOf<ResponceErrorDto,IdentityUser<int>>> CreateUserAsync(UserIntputDto userIntputDto)
    {
        
        var result = await _userManager!.CreateAsync(new IdentityUser<int>
        {
            UserName = userIntputDto.Username,
            Email = userIntputDto.Email
        }, userIntputDto.Password);
        
        if (!result.Succeeded)
        {

            return new ResponceErrorDto()
            {
                ErrorCode = "500",
                ErrorMessage = result.Errors.First().Description ?? "error registering"

            };
        }
        var user =  _userManager.FindByNameAsync(userIntputDto.Username);
        if (user.Result is not null)
        {
            try
            {
                if (userIntputDto.Role.ToLower() == RoleNames.Admin.ToLower())
                {
                    await _userManager.AddToRoleAsync(user.Result, RoleNames.Admin.ToUpper());
                }
                else
                {
                    await _userManager.AddToRoleAsync(user.Result, RoleNames.Customer.ToUpper());
                }
                return user.Result;
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user.Result);
                return new ResponceErrorDto()
                {
                    ErrorCode = "500",
                    ErrorMessage  = e.Message ?? "error registering"  
                };
            }
        }
        return new ResponceErrorDto()
        {
            ErrorCode = "500",
            ErrorMessage = "error registering"
        };
    }

    public async Task<OneOf<ResponceErrorDto, string>> LoginAsync(string username, string userPassword)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);
        if (user is not null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, userPassword, false, false);
            
            if (result.Succeeded)
            {
                
                //var token = await _userManager.GenerateUserTokenAsync(user,TokenOptions.DefaultProvider, "access_token");
                var token = _tokenUtil.GenerateTokenAsync(user).Result;
                
                return token;
            }
            else
            {
                return new ResponceErrorDto()
                {
                    ErrorCode = "500",
                    ErrorMessage = "Password incorrect"
                }; 
            }
        }

        return new ResponceErrorDto()
        {
            ErrorCode = "404",
            ErrorMessage = "User not found"
        };
    }
}