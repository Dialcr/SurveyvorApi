using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Dtos;
using BePrácticasLaborales.Services;
using BePrácticasLaborales.Services.UserServices;
using BePrácticasLaborales.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BePrácticasLaborales.Controllers;
[ApiController]
[Route("[Controller]")]

public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenUtil _tokenUtil;
    private readonly UserServicers _userServicers;

    public AuthController(UserManager<User> userManager, 
        SignInManager<User> signInManager, TokenUtil tokenUtil
        ,UserServicers userServicers)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenUtil = tokenUtil;
        _userServicers = userServicers;
        
    }
   

    [HttpGet]
    [Route("/test")]
    [Authorize(Roles = "ADMIN")]
    public IActionResult Testenpoint(string token, string userName )
    { 
        
        return Ok("good response");
        
    }
    [HttpGet]
    [Route("/test2")]
    public IActionResult Testenpoint2(string token, string userName )
    {
        return Ok("good response");
        
    }
    [HttpGet]
    [Route("/test3")]
    [AllowAnonymous]
    public IActionResult Testenpoint3(string token, string userName )
    {
        return Ok("good response");
        
    }
    
    [HttpPost]
    [Route("signin")]
    [AllowAnonymous]
    public async Task<IActionResult> Signin(
      string username, string userPassword)
    {
        var result = await _userServicers.LoginAsync(username, userPassword);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
     
    }
    
    [HttpPost]
    [Route("registrer")]
    [AllowAnonymous]
    public async Task<IActionResult> UserRegistrer(
        [FromBody] UserIntputDto userIntputDto)
    {
        /*
         var result = await _userManager!.CreateAsync(new IdentityUser<int>
        {
            UserName = userIntputDto.Username,
            Email = userIntputDto.Email
        }, userIntputDto.Password);
        
        if (!result.Succeeded)
        {
            
            foreach (var error in result.Errors)
            {
                throw new Exception("Register failed");
            }
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
                return Ok("registered");
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user.Result);
                BadRequest("cant add role, user not added");
            }
        }
        
        return BadRequest("error registering");
        */
        var result = await _userServicers.CreateUserAsync(userIntputDto,Url);
        if (result.TryPickT0(out var error, out var response))
        {
            
            return BadRequest(error);
        }
        return Ok(response) ;
    }
    
    [HttpPost]
    [Route("recovery/assword")]
    [AllowAnonymous]
    public async Task<IActionResult> RecoveryPassword(
        string email, string newPassword)
    {
        var result = await _userServicers.RecoveryPassword(email,newPassword,Url);
        if (result.TryPickT0(out var error, out var response))
        {
            
            return BadRequest(error);
        }
        return Ok(response) ;
    }
    
    
}

