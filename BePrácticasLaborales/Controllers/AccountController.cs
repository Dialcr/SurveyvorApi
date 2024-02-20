using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BePrácticasLaborales.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly UserServicers _userServices;

    public AccountController(UserManager<User> userManager, UserServicers userServices)
    {
        _userManager = userManager;
        _userServices = userServices;
        
    }
   

    [HttpGet]
    [Route("acount/confirmEmail/token")]
    [AllowAnonymous]
    public IActionResult ConfirmEmailToken(string token, string userName )
    { 
        return _userServices.ConfirmEmailToken(token, userName) ? Ok("confirmation success") : BadRequest("confirmation failed");
        
    }
    [HttpGet]
    [Route("acount/reset/password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(string userEmail, string newPassword, string token)
    { 
        //return _userServices.ConfirmEmailToken(token, userName) ? Ok("confirmation success") : BadRequest("confirmation failed");
        var result = await _userServices.ResetPassword(userEmail,  newPassword, token ) ;
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
}