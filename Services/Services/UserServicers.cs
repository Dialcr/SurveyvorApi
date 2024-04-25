using System.Net.Mail;
using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using Services.Dtos;
using Services.Services.EmailServices;
using Services.Settings;
using Services.Utils;

namespace Services.Services;

public class UserServicers : CustomServiceBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenUtil _tokenUtil;
    private readonly MailSettings _mailSettings;
    private readonly EmailService _emailServices;

    public UserServicers(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        TokenUtil tokenUtil,
        EntityDbContext context,
        IOptions<MailSettings> mailSettings,
        EmailService emailServices
    )
        : base(context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenUtil = tokenUtil;
        _mailSettings = mailSettings.Value;
        _emailServices = emailServices;
    }

    public async Task<OneOf<ResponseErrorDto, UserOutputDto>> CreateUserAsync(
        UserIntputDto userIntputDto,
        IUrlHelper urlHelper,
        string accountController
    )
    {
        var result = await _userManager!.CreateAsync(
            new User()
            {
                UserName = userIntputDto.Name,
                Email = userIntputDto.Email,
                OrganizationId = userIntputDto.OrganizationId,
                Image =
                    userIntputDto.Image
                    ?? $"http://gravatar.com/avatar/${{md5({userIntputDto.Name})}}?d=identicon"
            },
            userIntputDto.Password
        );

        if (!result.Succeeded)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 400,
                ErrorMessage = result.Errors.First().Description ?? "error registering"
            };
        }
        var user = await _userManager.FindByNameAsync(userIntputDto.Name);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

        if (user is not null)
        {
            try
            {
                if (user.OrganizationId is null)
                {
                    return new ResponseErrorDto()
                    {
                        ErrorCode = 400,
                        ErrorMessage =
                            "Error the user cant have organization role and have not organization assigned"
                    };
                }
                await _userManager.AddToRoleAsync(user, RoleNames.Organization.ToUpper());

#if DEBUG
                var templatePath = "./../Services/Services/Template/ConfirmEmailTemplate.html";
#else
                templatePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "./ConfirmEmailTemplate.html"
                );
#endif
                //SendEmailConfrim(user,urlHelper,token);
                var mailMessage = new MailMessage(_mailSettings.From, user.Email!);
                mailMessage.Subject = "Confirmation email account";
                //esto es nuevo
                mailMessage.IsBodyHtml = true;
                //hasta aqui
                var httpContext = new HttpContextAccessor().HttpContext;
                if (httpContext != null)
                {
                    /*var confirmationLink = urlHelper.Action(accountController,
                        "Account",
                        new { token, userName  = user.UserName },
                        httpContext.Request.Scheme, _mailSettings.UrlWEB);
                    var content = File.ReadAllText(templatePath);
                    var resultMessage = content.Replace("{{ConfirmationLink}}",confirmationLink);
                    */
                    var content = File.ReadAllText(templatePath);
                    var userName = content.Replace("{{UserName}}", user.UserName);
                    var resultMessage = userName.Replace(
                        "{{SupportEmailAddress}}",
                        _mailSettings.From
                    );

                    mailMessage.Body = resultMessage;
                }
                _emailServices.SendEmail(mailMessage, token);

                var userOutput = user.ToUserOutputDto();
                var role = await _userManager.GetRolesAsync(user);
                userOutput.Role = role[0];
                return userOutput;
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user);
                return new ResponseErrorDto()
                {
                    ErrorCode = 400,
                    ErrorMessage = e.Message ?? "error registering"
                };
            }
        }

        return new ResponseErrorDto() { ErrorCode = 400, ErrorMessage = "error registering" };
    }

    public async Task<OneOf<ResponseErrorDto, UserOutputDto>> EditUser(UserIntputDto userIntputDto)
    {
        var user = _userManager!.FindByNameAsync(userIntputDto.Name).Result;
        if (user is null)
        {
            return new ResponseErrorDto() { ErrorCode = 404, ErrorMessage = "User not found" };
        }

        user.Email = userIntputDto.Email;
        user.OrganizationId = userIntputDto.OrganizationId;
        user.Image = userIntputDto.Image ?? user.Image;

        var userOutput = user.ToUserOutputDto();
        var role = await _userManager.GetRolesAsync(user);
        userOutput.Role = role[0];
        return userOutput;
    }

    public async Task<OneOf<ResponseErrorDto, AuthResponseDtoOutput>> LoginAsync(
        string username,
        string userPassword
    )
    {
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);
        if (user is not null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, userPassword, false, false);

            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                var token = await _tokenUtil.GenerateTokenAsync(user);
                return new AuthResponseDtoOutput()
                {
                    User = new UserOutputDto()
                    {
                        Name = user.UserName!,
                        Email = user.Email!,
                        Role = role[0]
                    },
                    Authentication = new Jwt() { AccessToken = token },
                    UserId = user.Id
                };
            }

            return new ResponseErrorDto() { ErrorCode = 400, ErrorMessage = "Password incorrect" };
        }
        return new ResponseErrorDto() { ErrorCode = 404, ErrorMessage = "User not found" };
    }

    public OneOf<ResponseErrorDto, List<User>> ListUser()
    {
        var response = _userManager.Users.ToList();
        if (response.IsNullOrEmpty() || response.Count > 0)
        {
            return response;
        }
        return new ResponseErrorDto() { ErrorCode = 400, ErrorMessage = "Not user regitred" };
    }

    public async Task<OneOf<ResponseErrorDto, User>> GetUserById(int userId)
    {
        var response = await _userManager
            .Users.Include(x => x.Organization)
            .SingleOrDefaultAsync(x => x.Id == userId);
        if (response is not null)
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with id {userId} not found"
        };
    }

    public async Task<OneOf<ResponseErrorDto, User>> GetUserByEmail(string userEmail)
    {
        var response = await _userManager.FindByEmailAsync(userEmail);
        if (response is not null)
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with Email {userEmail} not found"
        };
    }

    public async Task<OneOf<ResponseErrorDto, User>> GetUserByUserName(string userName)
    {
        var response = await _userManager.FindByNameAsync(userName);
        if (response is not null)
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with name {userName} not found"
        };
    }

    public async Task<OneOf<ResponseErrorDto, User>> GetUserByUserNameOrEmail(
        string userNameOrEmail
    )
    {
        var response =
            await _userManager.FindByNameAsync(userNameOrEmail)
            ?? await _userManager.FindByEmailAsync(userNameOrEmail);
        if (response is not null)
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with name {userNameOrEmail} not found"
        };
    }

    public async Task<OneOf<ResponseErrorDto, User>> EditUser(
        UserIntputDto userIntputDto,
        int userId
    )
    {
        var result = GetUserById(userId);

        if (result.Result.TryPickT0(out var error, out var response))
        {
            return error;
        }

        try
        {
            response.Email = userIntputDto.Email;
            response.OrganizationId = userIntputDto.OrganizationId;
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return response;
    }

    public async Task<OneOf<ResponseErrorDto, User>> ResetPassword(
        string userEmail,
        string newPassword,
        string token
    )
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is not null)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return new ResponseErrorDto()
                {
                    ErrorCode = 404,
                    ErrorMessage = $"Reset password failed"
                };
            }
            return user;
        }

        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with Email {userEmail} not found"
        };
    }

    public async Task<OneOf<ResponseErrorDto, string>> RecoveryPassword(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ResponseErrorDto()
                {
                    ErrorCode = 404,
                    ErrorMessage = $"User with Email {email} not found"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var mailMessage = new MailMessage(_mailSettings.From, user.Email!);
            mailMessage.Subject = "Reset Password";

            string urlConToken = $"{_mailSettings.UrlWEBFront}?token={token}";

#if DEBUG
            var templatePath = "./../Services/Services/Template/RecoveryPasswordTemplate.html";
#else
            templatePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "./RecoveryPasswordTemplate.html"
            );
#endif
            var content = File.ReadAllText(templatePath);
            var urlFront = content.Replace("{{SupportEmailAddress}}", urlConToken);

            mailMessage.Body = urlFront;
            mailMessage.IsBodyHtml = true;
            _emailServices.SendEmail(mailMessage, token);
            return token;
        }
        catch (Exception e)
        {
            return new ResponseErrorDto() { ErrorCode = 404, ErrorMessage = e.Message };
        }
    }

    public async Task<bool> confirmEmail(string token, string userName)
    {
        bool esc = true;
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
        if (user is not null)
        {
            var result = await _userManager.ConfirmEmailAsync(user!, token);
            if (!result.Succeeded)
            {
                esc = false;
            }
        }
        else
        {
            esc = false;
        }
        return esc;
    }

    public bool ConfirmEmailToken(string token, string userName)
    {
        bool esc = true;
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
        if (user is not null)
        {
            var result = _userManager.ConfirmEmailAsync(user!, token);
            if (!result.Result.Succeeded)
            {
                esc = false;
            }
        }
        else
        {
            esc = false;
        }
        return esc;
    }
}
