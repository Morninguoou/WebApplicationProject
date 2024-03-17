using Microsoft.AspNetCore.Mvc;
using marian_onsite.Models;
using marian_onsite.Services;

using Microsoft.AspNetCore.Authorization;

namespace marian_onsite.Controllers;

[Authorize]
[ApiController]
[Route("api/email")]
public class EmailAPIController : Controller
{
    private readonly EmailService _emailService;
    private readonly ILogger<EmailAPIController> _logger;

    public EmailAPIController(
        ILogger<EmailAPIController> logger,
        EmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmailAsync(SendEmail sendEmail)
    {
        try
        {
            await _emailService.SendEmailAsync(sendEmail.Email, sendEmail.Subject, sendEmail.Body);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}