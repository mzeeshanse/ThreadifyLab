using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ThreadifyLab.Models;
using ThreadifyLab.Repositories;
using ThreadifyLab.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ThreadifyLab.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IAdminService _adminService;

    public AdminController(
        IServiceRepository serviceRepository,
        IPortfolioRepository portfolioRepository,
        IContactRepository contactRepository,
        IAdminService adminService)
    {
        _serviceRepository = serviceRepository;
        _portfolioRepository = portfolioRepository;
        _contactRepository = contactRepository;
        _adminService = adminService;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Username and password are required.";
            return View();
        }

        var isValid = await _adminService.ValidateCredentialsAsync(username, password);
        if (!isValid)
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.UnreadMessages = await _contactRepository.GetUnreadCountAsync();
        ViewBag.ServicesCount = (await _serviceRepository.GetAllAsync()).Count();
        ViewBag.PortfolioCount = (await _portfolioRepository.GetAllAsync()).Count();
        return View();
    }

    // Services Management
    public async Task<IActionResult> Services()
    {
        var services = await _serviceRepository.GetAllAsync();
        return View(services);
    }

    public IActionResult CreateService()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateService(Service service)
    {
        if (ModelState.IsValid)
        {
            await _serviceRepository.CreateAsync(service);
            return RedirectToAction("Services");
        }
        return View(service);
    }

    public async Task<IActionResult> EditService(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null) return NotFound();
        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditService(Service service)
    {
        if (ModelState.IsValid)
        {
            await _serviceRepository.UpdateAsync(service);
            return RedirectToAction("Services");
        }
        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteService(int id)
    {
        await _serviceRepository.DeleteAsync(id);
        return RedirectToAction("Services");
    }

    // Portfolio Management
    public async Task<IActionResult> Portfolio()
    {
        var portfolio = await _portfolioRepository.GetAllAsync();
        return View(portfolio);
    }

    public IActionResult CreatePortfolio()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePortfolio(PortfolioItem item)
    {
        if (ModelState.IsValid)
        {
            await _portfolioRepository.CreateAsync(item);
            return RedirectToAction("Portfolio");
        }
        return View(item);
    }

    public async Task<IActionResult> EditPortfolio(int id)
    {
        var item = await _portfolioRepository.GetByIdAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPortfolio(PortfolioItem item)
    {
        if (ModelState.IsValid)
        {
            await _portfolioRepository.UpdateAsync(item);
            return RedirectToAction("Portfolio");
        }
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePortfolio(int id)
    {
        await _portfolioRepository.DeleteAsync(id);
        return RedirectToAction("Portfolio");
    }

    // Contact Messages
    public async Task<IActionResult> Messages()
    {
        var messages = await _contactRepository.GetAllAsync();
        return View(messages);
    }

    public async Task<IActionResult> ViewMessage(int id)
    {
        var message = await _contactRepository.GetByIdAsync(id);
        if (message == null) return NotFound();
        
        if (!message.IsRead)
        {
            await _contactRepository.MarkAsReadAsync(id);
        }
        
        return View(message);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        await _contactRepository.DeleteAsync(id);
        return RedirectToAction("Messages");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}

