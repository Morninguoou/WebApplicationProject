using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;
using marian_onsite.Services;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace marian_onsite.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]

public class UserAPIController : ControllerBase
{
    private readonly UserService _userService;
    private readonly StudyGroupService _studyGroupService;

    public UserAPIController(UserService userService, StudyGroupService studyGroupService)
    {
        _userService = userService;
        _studyGroupService = studyGroupService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAsync()
    {
        var users = await _userService.GetAsync();
        return Ok(users);
    }

    [HttpGet("profile")]
    public async Task<ActionResult> GetProfileAsync(ClaimsPrincipal currentUser)
    {
        var user = await _userService.GetAsync(currentUser.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("study-groups")]
    public async Task<ActionResult<List<StudyGroup>>> GetStudyGroupsAsync(ClaimsPrincipal currentUser)
    {
        var user = await _userService.GetAsync(currentUser.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        var studyGroups = await _studyGroupService.FindByUserIdAsync(user.Id);
        return Ok(studyGroups);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateAsync(User newUser)
    {
        await _userService.CreateAsync(newUser);
        return CreatedAtAction(nameof(GetAsync), new { id = newUser.Id }, newUser);
    }
}