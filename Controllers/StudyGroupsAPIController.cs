using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using marian_onsite.Models;
using marian_onsite.Services;
using marian_onsite.Enums;

namespace marian_onsite.Controllers;
[Authorize]
[ApiController]
[Route("api/study-groups")]
public class StudyGroupsAPIController : ControllerBase
{
    private readonly StudyGroupService _studyGroupService;
    private readonly UserService _userService;
    private readonly EmailService _emailService;

    private readonly RequestJoinStudyGroupService _requestJoinStudyGroupService;


    public StudyGroupsAPIController(StudyGroupService studyGroup, UserService userService, EmailService emailService, RequestJoinStudyGroupService requestJoinStudyGroupService)
    {
        _studyGroupService = studyGroup;
        _userService = userService;
        _emailService = emailService;
        _requestJoinStudyGroupService = requestJoinStudyGroupService;
    }

    /*
    * GET /api/study-groups
    * Get all studygroups
    */
    [HttpGet]
    public async Task<ActionResult<List<StudyGroup>>> GetAsync()
    {
        var studygroups = await _studyGroupService.GetAsync();
        return Ok(studygroups);
    }

    /*
    * GET /api/study-groups/{id}
    * Get a studygroup by id
    */
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        var studyGroup = await _studyGroupService.GetAsync(id);
        if (studyGroup == null)
        {
            return NotFound();
        }

        var creator = await _userService.GetAsync(studyGroup.Creator);

        var user = await _userService.GetAsync(User.Identity.Name);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var groupMembers = new List<GroupMember>();

        foreach (var member in studyGroup.Members)
        {
            var _member = await _userService.GetAsync(member);
            groupMembers.Add(new GroupMember
            {
                Id = _member.Id,
                AvatarUrl = _member.AvatarUrl,
                Username = _member.Username,
                IsFollowing = _member.Followers.Contains(user.Id)
            });
        }

        return Ok(new
        {
            studyGroup,
            creator = new
            {
                Id = creator.Id,
                AvatarUrl = creator.AvatarUrl,
                Username = creator.Username,
                IsFollowing = creator.Followers.Contains(user.Id)
            }
        });
    }

    /*
    * POST /api/study-groups
    * Create a new studygroup
    */
    [HttpPost]
    public async Task<ActionResult<StudyGroup>> CreateAsync(StudyGroup newStudyGroup, ClaimsPrincipal currentUser)
    {
        var user = await _userService.GetAsync(currentUser.Identity.Name);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        // Set the creator of the study group to the current user
        newStudyGroup.Creator = user.Id;

        // Create the study group
        await _studyGroupService.CreateAsync(newStudyGroup);


        // send an email to the creator follower when a new study group is created
        var followers = await _userService.FindFollowersAsync(user.Id);

        foreach (var follower in followers)
        {
            var _follower = await _userService.GetAsync(follower.Id);

            await _emailService.SendEmailAsync(
                _follower.Email,
                $"Marian On-site | New Study Group Created by {user.Username}",
                $"A new study group {newStudyGroup.Title}\n\nDescription: {newStudyGroup.Description}\n\nhas been created by {user.Username}.\n\nJoin the study group now to start learning together!"
            );
        }

        return Ok(newStudyGroup);
    }

    /*
    * PUT /api/study-groups/{id}
    * Update a studygroup by id
    */
    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> UpdateAsync(string id, StudyGroup updatedStudygroup, ClaimsPrincipal currentUser)
    {


        var studygroup = await _studyGroupService.GetAsync(id);
        if (studygroup == null)
        {
            return NotFound();
        }

        if (studygroup.Creator != User.Identity.Name)
        {
            return Unauthorized("You are not the owner of the study group");
        }

        updatedStudygroup.Id = studygroup.Id;
        await _studyGroupService.UpdateAsync(studygroup.Id, updatedStudygroup);
        return NoContent();
    }

    /*
    * DELETE /api/study-groups/{id}
    * Delete a studygroup by id
    */
    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> DeleteAsync(string id, ClaimsPrincipal currentUser)
    {
        var studygroup = await _studyGroupService.GetAsync(id);
        if (studygroup == null)
        {
            return NotFound();
        }

        if (studygroup.Creator != User.Identity.Name)
        {
            return Unauthorized("You are not the owner of the study group");
        }

        await _studyGroupService.DeleteAsync(studygroup.Id);
        return NoContent();
    }

    [HttpGet("{studyGroupId:length(24)}/request-joins")]
    public async Task<ActionResult<List<RequestJoinStudyGroup>>> GetRequestJoinAsync(string studyGroupId)
    {
        var studygroup = await _studyGroupService.GetAsync(studyGroupId);
        if (studygroup == null)
        {
            return NotFound("Study group not found");
        }

        if (studygroup.Creator != User.Identity.Name)
        {
            return Unauthorized("You are not the owner of the study group");
        }

        var requests = await _requestJoinStudyGroupService.GetByStudyGroupIdAsync(studyGroupId);
        return Ok(requests);
    }

    [HttpPost("{studyGroupId:length(24)}/request-joins")]
    public async Task<ActionResult> RequestJoinAsync(string studyGroupId, ClaimsPrincipal currentUser)
    {
        var user = await _userService.GetAsync(currentUser.Identity.Name);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var studygroup = await _studyGroupService.GetAsync(studyGroupId);
        if (studygroup == null)
        {
            return NotFound("Study group not found");
        }

        // if (studygroup.Creator == user.Id)
        // {
        //     return BadRequest("You are the owner of the study group");
        // }

        var request = new RequestJoinStudyGroup
        {
            StudyGroupId = studyGroupId,
            UserId = user.Id,
            Status = RequestJoinStudyGroupStatus.Pending
        };

        await _requestJoinStudyGroupService.CreateAsync(request);

        return Ok(request);
    }

    [HttpPut("{studyGroupId:length(24)}/request-joins/accept/{id:length(24)}", Name = "UpdateRequestJoinAccept")]
    public async Task<ActionResult> UpdateRequestJoinAsync(string studyGroupId, string id, ClaimsPrincipal currentUser)
    {
        var request = await _requestJoinStudyGroupService.GetAsync(id);
        if (request == null)
        {
            return NotFound("Request not found");
        }

        if (request.StudyGroupId != studyGroupId)
        {
            return BadRequest("Study group id does not match");
        }

        var studygroup = await _studyGroupService.GetAsync(studyGroupId);
        if (studygroup == null)
        {
            return NotFound("Study group not found");
        }

        if (studygroup.Creator != currentUser.Identity.Name)
        {
            return Unauthorized("You are not the owner of the study group");
        }

        var updatedRequestJoinStudyGroup = new RequestJoinStudyGroup
        {
            Id = request.Id,
            Status = RequestJoinStudyGroupStatus.Accepted
        };

        await _requestJoinStudyGroupService.UpdateAsync(request.Id, updatedRequestJoinStudyGroup);

        var user = await _userService.GetAsync(request.UserId);
        // send an email to the user when the request is accepted
        await _emailService.SendEmailAsync(
            user.Email,
            "Marian On-site | Request to join study group accepted",
            $"Your request to join the study group '{studygroup.Title}' has been accepted."
        );

        return NoContent();
    }

    [HttpPut("{studyGroupId:length(24)}/request-joins/reject/{id:length(24)}", Name = "UpdateRequestJoinReject")]
    public async Task<ActionResult> RejectRequestJoinAsync(string studyGroupId, string id, ClaimsPrincipal currentUser)
    {
        var request = await _requestJoinStudyGroupService.GetAsync(id);
        if (request == null)
        {
            return NotFound();
        }

        if (request.StudyGroupId != studyGroupId)
        {
            return BadRequest("Study group id does not match");
        }

        var studygroup = await _studyGroupService.GetAsync(studyGroupId);
        if (studygroup == null)
        {
            return NotFound("Study group not found");
        }

        if (studygroup.Creator != currentUser.Identity.Name)
        {
            return Unauthorized("You are not the owner of the study group");
        }

        var updatedRequestJoinStudyGroup = new RequestJoinStudyGroup
        {
            Id = request.Id,
            Status = RequestJoinStudyGroupStatus.Rejected
        };
        await _requestJoinStudyGroupService.UpdateAsync(request.Id, updatedRequestJoinStudyGroup);

        var user = await _userService.GetAsync(request.UserId);
        // send an email to the user when the request is rejected
        await _emailService.SendEmailAsync(
            user.Email,
            "Marian On-site | Request to join study group rejected",
            $"Your request to join the study group {studygroup.Title} has been rejected."
        );
        return NoContent();
    }


}
