using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;
using marian_onsite.Services;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace marian_onsite.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly EmailService _emailService;

    // dependency injection
    public UsersController(UserService userService, EmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    /*
    * GET /api/users
    * Get all users
    */
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAsync()
    {
        var users = await _userService.GetAsync();
        return Ok(users);
    }

    /*
    * GET /api/users/{id}
    * Get a user by id
    */
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> GetAsync(string id)
    {
        var user = await _userService.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost("{id:length(24)}/follow")]
    public async Task<IActionResult> FollowAsync(string id, ClaimsPrincipal currentUser)
    {
        if (id == currentUser.Identity?.Name)
        {
            return BadRequest("You cannot follow yourself");
        }


        var userTarget = await _userService.GetAsync(id);
        if (userTarget == null)
        {
            return NotFound();
        }


        var followeeId = currentUser.Identity?.Name;
        if (followeeId == null)
        {
            return BadRequest("Invalid followee ID");
        }

        var followee = await _userService.GetAsync(followeeId);

        if (followee.Following.Contains(userTarget.Id))
        {
            return BadRequest("You are already following this user");
        }

        await _userService.AddFollowerAsync(userTarget.Id, followeeId);
        await _userService.AddFollowingAsync(followee.Id, userTarget.Id);

        await this._emailService.SendEmailAsync(userTarget.Email, "Marian On-site | New Follower", $"{followee.Username} is now following you");

        return Ok();
    }

    [HttpPost("{id:length(24)}/unfollow")]
    public async Task<ActionResult<User>> UnfollowAsync(string id, ClaimsPrincipal currentUser)
    {
        if (id == currentUser.Identity?.Name)
        {
            return BadRequest("You cannot unfollow yourself");
        }

        var userTarget = await _userService.GetAsync(id);
        if (userTarget == null)
        {
            return NotFound();
        }

        var followeeId = currentUser.Identity?.Name;
        if (followeeId == null)
        {
            return BadRequest("Invalid followee ID");
        }

        var followee = await _userService.GetAsync(followeeId);

        if (!followee.Following.Contains(userTarget.Id))
        {
            return BadRequest("You are not following this user");
        }

        await _userService.RemoveFollowerAsync(userTarget.Id, followeeId);
        await _userService.RemoveFollowingAsync(followee.Id, userTarget.Id);

        return Ok();
    }

    /*
    * PUT /api/users/{id}
    * Update a user by id
    */
    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> UpdateAsync(string id, User updatedUser)
    {
        var user = await _userService.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;
        if (updatedUser.Id != null) // Add null check for 'id' parameter
        {
            await _userService.UpdateAsync(updatedUser.Id, updatedUser);
        }
        return NoContent();
    }

    /*
    * DELETE /api/users/{id}
    * Delete a user by id
    */
    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var user = await _userService.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        if (user.Id != null) // Add null check for 'id' parameter
        {
            await _userService.DeleteAsync(user.Id);
        }
        return NoContent();
    }
}
