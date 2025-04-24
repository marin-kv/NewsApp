using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Models.DB;

namespace NewsApp.Controllers;

[ApiController]
[Authorize]
public class AuthController(SignInManager<User> signInManager) : ControllerBase
{
    private readonly SignInManager<User> _signInManager = signInManager;
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}
