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
    private readonly ILogger<UserServicers> _logger;

    public UserServicers(UserManager<IdentityUser<int>> userManager, 
        SignInManager<IdentityUser<int>> signInManager, TokenUtil tokenUtil
        ,ILogger<UserServicers> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenUtil = tokenUtil;
        _logger = logger;
    }

    public async Task<OneOf<ResponseErrorDto,IdentityUser<int>>> CreateUserAsync(UserIntputDto userIntputDto)
    {
        
        var result = await _userManager!.CreateAsync(new IdentityUser<int>
        {
            UserName = userIntputDto.Username,
            Email = userIntputDto.Email
        }, userIntputDto.Password);
        
        if (!result.Succeeded)
        {

            _logger.LogError(result.Errors.First().Description ?? "error registering");
            return new ResponseErrorDto()
            {
                ErrorCode = "500",
                ErrorMessage = result.Errors.First().Description ?? "error registering"

            };
        }
        _logger.LogInformation($"Added {userIntputDto.Username} with email: {userIntputDto.Email} " +
                               $"at date {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
        
        var user =  _userManager.FindByNameAsync(userIntputDto.Username);
        if (user.Result is not null)
        {
            try
            {
                if (userIntputDto.Role.ToLower() == RoleNames.Admin.ToLower())
                {
                    _logger.LogInformation($"Added {RoleNames.Admin} role to {user.Result.UserName} with email: {user.Result.Email}");
                    await _userManager.AddToRoleAsync(user.Result, RoleNames.Admin.ToUpper());
                }
                else
                {
                    _logger.LogInformation($"Added {RoleNames.Customer} role to {user.Result.UserName} with email: {user.Result.Email}");
                    await _userManager.AddToRoleAsync(user.Result, RoleNames.Customer.ToUpper());
                }
                return user.Result;
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user.Result);
                _logger.LogError(e.Message ?? "error registering");
                return new ResponseErrorDto()
                {
                    ErrorCode = "500",
                    ErrorMessage  = e.Message ?? "error registering"  
                };
            }
        }

        _logger.LogError("error registering");
        return new ResponseErrorDto()
        {
            ErrorCode = "500",
            ErrorMessage = "error registering"
        };
    }

    public async Task<OneOf<ResponseErrorDto, string>> LoginAsync(string username, string userPassword)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);
        if (user is not null)
        {
            //authenticated
            _logger.LogInformation($"Login {user.UserName} with email: {user.Email} " +
                                   $"at date {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
            var result = await _signInManager.PasswordSignInAsync(user, userPassword, false, false);
            
            if (result.Succeeded)
            {
                
                //var token = await _userManager.GenerateUserTokenAsync(user,TokenOptions.DefaultProvider, "access_token");
                var token = _tokenUtil.GenerateTokenAsync(user).Result;
                //authenticated
            _logger.LogInformation($"Login {user.UserName} with email: {user.Email} " +
                                   $"at date {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
            
                return token;
            }
            _logger.LogError($"error into login {user.UserName}, password incorrect");
             return new ResponseErrorDto()
                {
                    ErrorCode = "500",
                    ErrorMessage = "Password incorrect"
                }; 
        
        }
        _logger.LogError($"user {user!.UserName} not found");
        return new ResponseErrorDto()
        {
            ErrorCode = "404",
            ErrorMessage = "User not found"
        };
    }
}