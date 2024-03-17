using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;
using marian_onsite.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Security.Claims;
using System.Diagnostics;

namespace marian_onsite.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthAPIController : Controller
{
    private readonly ILogger<AuthAPIController> _logger;
    private readonly UserService _userService;

    public AuthAPIController(
        ILogger<AuthAPIController> logger,
        UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IResult> RegisterAsync(User newUserData)
    {
        var userEmailExisting = await _userService.FindOneByEmailAsync(newUserData.Email);
        if (userEmailExisting != null)
        {
            return Results.Conflict(
                new { message = "Email already exists" }
            );
        }

        var userUsernameExisting = await _userService.FindOneByUsernameAsync(newUserData.Username);
        if (userUsernameExisting != null)
        {
            return Results.Conflict(
                new { message = "Username already exists" }
            );
        }

        newUserData.Password = BCrypt.Net.BCrypt.HashPassword(newUserData.Password);

        await _userService.CreateAsync(newUserData);

        var newUser = await _userService.FindOneByEmailAsync(newUserData.Email);

        var claimsPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, newUser.Id) },
                BearerTokenDefaults.AuthenticationScheme
            )
        );

        return Results.SignIn(claimsPrincipal);
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IResult> LoginAsync(Login loginModel)
    {
        {
            var user = await _userService.FindOneByUsernameAsync(loginModel.Username);
            if (user == null)
            {
                return Results.NotFound(
                    new { message = "User not found" }
                );
            }

            if (!BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                return Results.Unauthorized(); 
            }

            var claimsPrincipal = new ClaimsPrincipal(
              new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, user.Id) },
                BearerTokenDefaults.AuthenticationScheme
              )
            );

            return Results.SignIn(claimsPrincipal);
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IResult> Me(ClaimsPrincipal user)
    {
        var currentUser = await _userService.GetAsync(user.Identity?.Name);
        if (currentUser == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(currentUser);
    }


    [Authorize]
    [HttpPost("logout")]
    public IResult Logout(ClaimsPrincipal user)
    {
        return Results.SignOut();
    }

    [Authorize]
    [HttpPost("refresh")]
    public IResult Refresh()
    {
        return Results.Ok();
    }
}