using System.Net.Mail;
using BePrácticasLaborales.Controllers;
using BePrácticasLaborales.DataAcces;
using BePrácticasLaborales.Dtos;
using BePrácticasLaborales.Services.EmailServices;
using BePrácticasLaborales.Settings;
using BePrácticasLaborales.Utils;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OneOf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;


namespace BePrácticasLaborales.Services.UserServices;

public class UserServicers 
{
    
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly TokenUtil _tokenUtil;
    private readonly EntityDbContext _context;
    private readonly MailSettings _mailSettings;
    private readonly EmailService _emailServices;
    


    public UserServicers(UserManager<User> userManager, 
        SignInManager<User> signInManager, TokenUtil tokenUtil,
        EntityDbContext context,
        IOptions <MailSettings> mailSettings,
        EmailService emailServices)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenUtil = tokenUtil;
        _context = context;
        _mailSettings = mailSettings.Value;
        _emailServices = emailServices;
    }

    public async Task<OneOf<ResponseErrorDto,User>> CreateUserAsync(UserIntputDto userIntputDto, IUrlHelper urlHelper)
    {   
        var result = await _userManager!.CreateAsync(new User()
        {
            UserName = userIntputDto.Username,
            Email = userIntputDto.Email,
            OrganizationId = userIntputDto.OrganizationId
        }, userIntputDto.Password);
        
        if (!result.Succeeded)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 400,
                ErrorMessage = result.Errors.First().Description ?? "error registering"

            };
        }
        var user =  await _userManager.FindByNameAsync(userIntputDto.Username);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

        if (user is not null)
        {
            try
            {
                if (userIntputDto.Role.ToLower() == RoleNames.Admin.ToLower())
                {
                    if (user.OrganizationId is not null)
                    {
                        return new ResponseErrorDto()
                        {
                            ErrorCode = 400,
                            ErrorMessage = "Error the user cant have admin role and organization"
                        };
                    }
                    await _userManager.AddToRoleAsync(user, RoleNames.Admin.ToUpper());
                    
                }
                else
                {
                    if (user.OrganizationId is null)
                    {
                        return new ResponseErrorDto()
                        {
                            ErrorCode = 400,
                            ErrorMessage = "Error the user cant have organization role and have not organization assigned"
                        };
                    }
                    await _userManager.AddToRoleAsync(user, RoleNames.Organization.ToUpper());
                    /*
                    var mailMessage = new MailMessage(_mailSettings.From, user.Email!);
                    mailMessage.Subject = "Confirmación de cuenta";
                    var httpContext = .HttpContext;
                    if (httpContext is not null)
                    {
                        var confirmationLink = _urlHelper.Action(nameof(ConfirmEmailToken),
                            "Account",
                            new { token, userName  = user.UserName },
                            httpContext.Request.Scheme, _mailSettings.UrlWEB);
                        mailMessage.Body = $"Para confirmar su cuenta, haga click en el siguiente enlace:\n\n{confirmationLink}\n\naqui deberia de haber un link";

                        await _emailServices.SendEmail(mailMessage,token);
                    }
                    */
                   
                }
                //SendEmailConfrim(user,urlHelper,token);
                var mailMessage = new MailMessage(_mailSettings.From, user.Email!);
                mailMessage.Subject = "Confirmación de cuenta";
                var httpContext = new HttpContextAccessor().HttpContext;
                if (httpContext is not null)
                {
                    var confirmationLink = urlHelper.Action(nameof(AccountController.ConfirmEmailToken), 
                        "Account", 
                        new { token, userName  = user.UserName },
                        httpContext.Request.Scheme, _mailSettings.UrlWEB);
                    mailMessage.Body = $"Para confirmar su cuenta, haga click en el siguiente enlace:\n\n{confirmationLink}\n\naqui deberia de haber un link";
            
                    _emailServices.SendEmail(mailMessage,token);
                }

                return user;
            }
            catch (Exception e)
            {
                await _userManager.DeleteAsync(user);
                return new ResponseErrorDto()
                {
                    ErrorCode = 400,
                    ErrorMessage  = e.Message ?? "error registering"  
                };
            }
        }
        
        return new ResponseErrorDto()
        {
            ErrorCode = 400,
            ErrorMessage = "error registering"
        };
    }

    public async Task<OneOf<ResponseErrorDto, string>> LoginAsync(string username, string userPassword)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == username);
        if (user is not null)
        {
            
            var result = await _signInManager.PasswordSignInAsync(user, userPassword, false, false);
            
            if (result.Succeeded)
            {
                var token =  await _tokenUtil.GenerateTokenAsync(user);
                return token;
            }
            
            return new ResponseErrorDto()
            {
                ErrorCode = 400,
                ErrorMessage = "Password incorrect"
            }; 
        
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = "User not found"
        };
    }

    public OneOf<ResponseErrorDto, List<User>> ListUser()
    {
        var response = _userManager.Users.ToList();
        if (response.IsNullOrEmpty() || response.Count > 0)
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 400,
            ErrorMessage = "Not user regitred"
        };
    }
    public async Task<OneOf<ResponseErrorDto, User>> GetUserById(int userId)
    {
        var response = await _userManager.Users.Include(x=>x.Organization)
            .SingleOrDefaultAsync(x=> x.Id == userId);
        if (response is not null )
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
        if (response is not null )
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
        if (response is not null )
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with name {userName} not found"
        };
    }
    
    public async Task<OneOf<ResponseErrorDto, User>> GetUserByUserNameOrEmail(string userNameOrEmail)
    {
        var response = await _userManager.FindByNameAsync(userNameOrEmail) ??
                       await _userManager.FindByEmailAsync(userNameOrEmail);
        if (response is not null )
        {
            return response;
        }
        return new ResponseErrorDto()
        {
            ErrorCode = 404,
            ErrorMessage = $"User with name {userNameOrEmail} not found"
        };
    }
    
    public async Task<OneOf<ResponseErrorDto, User>> EditUser(UserIntputDto userIntputDto, int userId)
    {
        var result = GetUserById(userId);
        
        if (result.Result.TryPickT0(out var error, out var response))
        {
            return error;
        }

        try
        {
            //todo: provar que esto funcinoa 
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

    public async Task<OneOf<ResponseErrorDto, User>> ResetPassword(string userEmail, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is not null)
        {
            //var newToken = await _userManager.GenerateUserTokenAsync(user.Result, TokenOptions.DefaultProvider, "recovery_password");
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
    public async Task<OneOf<ResponseErrorDto, string>> RecoveryPassword( string email, string newPassword, IUrlHelper urlHelper)
    {
        //todo: no se si esto funciona
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
            //var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "recovery_password");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var mailMessage = new MailMessage(_mailSettings.From, user.Email!);
            mailMessage.Subject = "Reset Password";
            var httpContext = new HttpContextAccessor().HttpContext;
            if (httpContext is not null)
            {
                var confirmationLink = urlHelper.Action(nameof(AccountController.ResetPassword), 
                    "Account", 
                    new { userEmail=email,token,newPassword },
                    httpContext.Request.Scheme, _mailSettings.UrlWEB);
                mailMessage.Body = $"Para confirmar el cambnio de contraseña, haga click en el siguiente enlace:\n\n{confirmationLink}\n\naqui deberia de haber un link";
            
                _emailServices.SendEmail(mailMessage,token);
            }

            return token;
        }
        catch (Exception e)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = e.Message
            };
        }
    }
    
    public async Task<bool> confirmEmail(string token, string userName)
    {
        bool esc = true;
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
        if (user is not null)
        {
            var result =  await _userManager.ConfirmEmailAsync(user!,token);
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
    public  bool ConfirmEmailToken(string token, string userName)
    {
        bool esc = true;
        var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
        if (user is not null)
        {
            var result =   _userManager.ConfirmEmailAsync(user!,token);
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
    public bool SendEmailConfrim(User user, IUrlHelper urlHelper,string token )
    {
        var mailMessage = new MailMessage(_mailSettings.From, user.Email!);
        mailMessage.Subject = "Confirmación de cuenta";
        var httpContext = new HttpContextAccessor().HttpContext;
        if (httpContext is not null)
        {
            var confirmationLink = urlHelper.Action(nameof(AccountController.ConfirmEmailToken), 
                "Account", 
                new { token, userName  = user.UserName },
                httpContext.Request.Scheme, _mailSettings.UrlWEB);
            mailMessage.Body = $"Para confirmar su cuenta, haga click en el siguiente enlace:\n\n{confirmationLink}\n\naqui deberia de haber un link";
            
            _emailServices.SendEmail(mailMessage,token);
        }

        return true;
    }
}