using BePrácticasLaborales.Middleware;
using DataAcces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Services;
using Services.Utils;

namespace BePrácticasLaborales.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenUtil _tokenUtil;
    private readonly UserServicers _userServicers;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        TokenUtil tokenUtil,
        UserServicers userServicers,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenUtil = tokenUtil;
        _userServicers = userServicers;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Route("signin")]
    [ProducesResponseType(typeof(AuthResponseDtoOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> Signin([FromBody] UserSignIn userSignIn)
    {
        var result = await _userServicers.LoginAsync(userSignIn.Username, userSignIn.UserPassword);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }

    [HttpPost]
    [Route("registrer")]
    [ProducesResponseType(typeof(UserOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    //[Authorize(Roles = "ADMIN")]
    [AllowAnonymous]
    public async Task<IActionResult> UserRegistrer([FromBody] UserIntputDto userIntputDto)
    {
        var accountController = nameof(AccountController.ConfirmEmailToken);

        var result = await _userServicers.CreateUserAsync(userIntputDto, Url, accountController);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }

    [HttpPatch]
    [Route("editUser")]
    [ProducesResponseType(typeof(UserOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN")]
    [Authorize(Roles = "ORGANIZATION")]
    public async Task<IActionResult> EditUser([FromBody] UserIntputDto userIntputDto)
    {
        var result = await _userServicers.EditUser(userIntputDto);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }

    [HttpPost]
    [Route("recovery/password")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> RecoveryPassword(string email)
    {
        var result = await _userServicers.RecoveryPassword(email);
        if (result.TryPickT0(out var error, out var response))
        {
            return BadRequest(error);
        }
        return Ok(response);
    }
}
