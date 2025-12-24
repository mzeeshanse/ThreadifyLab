using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreadifyLab.Models;
using ThreadifyLab.Services;

namespace ThreadifyLab.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User model, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("confirmPassword", "Passwords do not match.");
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, message, user) = await _userService.RegisterAsync(model, password);

        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("RegisterSuccess");
        }

        ViewBag.Error = message;
        return View(model);
    }

    [HttpGet]
    public IActionResult RegisterSuccess()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, string returnUrl)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Email and password are required.";
            return View();
        }

        var (success, message, user) = await _userService.LoginAsync(email, password);

        if (!success)
        {
            ViewBag.Error = message;
            return View();
        }

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            ViewBag.Error = "Invalid verification token.";
            return View();
        }

        var success = await _userService.VerifyEmailAsync(token);
        
        if (success)
        {
            ViewBag.Success = true;
            ViewBag.Message = "Your email has been verified successfully! You can now log in.";
        }
        else
        {
            ViewBag.Success = false;
            ViewBag.Message = "Invalid or expired verification token. Please request a new verification email.";
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendVerification(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            TempData["Error"] = "Email is required.";
            return RedirectToAction("Login");
        }

        var success = await _userService.ResendVerificationEmailAsync(email);
        
        if (success)
        {
            TempData["SuccessMessage"] = "Verification email has been sent. Please check your inbox.";
        }
        else
        {
            TempData["Error"] = "Unable to resend verification email. Please check your email address or contact support.";
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Dashboard()
    {
        return View();
    }
}









