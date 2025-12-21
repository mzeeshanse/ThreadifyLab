using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ThreadifyLab.Models;
using ThreadifyLab.Repositories;
using ThreadifyLab.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ThreadifyLab.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IAdminService _adminService;
    private readonly ICapLogoRepository _capLogoRepository;
    private readonly IPatchRepository _patchRepository;
    private readonly IJacketBackRepository _jacketBackRepository;
    private readonly IChestLogoRepository _chestLogoRepository;
    private readonly IBadgeLogoRepository _badgeLogoRepository;
    private readonly IVectorArtRepository _vectorArtRepository;

    public AdminController(
        IServiceRepository serviceRepository,
        IPortfolioRepository portfolioRepository,
        IContactRepository contactRepository,
        IAdminService adminService,
        ICapLogoRepository capLogoRepository,
        IPatchRepository patchRepository,
        IJacketBackRepository jacketBackRepository,
        IChestLogoRepository chestLogoRepository,
        IBadgeLogoRepository badgeLogoRepository,
        IVectorArtRepository vectorArtRepository)
    {
        _serviceRepository = serviceRepository;
        _portfolioRepository = portfolioRepository;
        _contactRepository = contactRepository;
        _adminService = adminService;
        _capLogoRepository = capLogoRepository;
        _patchRepository = patchRepository;
        _jacketBackRepository = jacketBackRepository;
        _chestLogoRepository = chestLogoRepository;
        _badgeLogoRepository = badgeLogoRepository;
        _vectorArtRepository = vectorArtRepository;
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

    // Cap Logo Management
    public async Task<IActionResult> CapLogos()
    {
        var capLogos = await _capLogoRepository.GetAllAsync();
        return View(capLogos);
    }

    public IActionResult CreateCapLogo()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCapLogo(CapLogo capLogo)
    {
        if (ModelState.IsValid)
        {
            await _capLogoRepository.CreateAsync(capLogo);
            return RedirectToAction("CapLogos");
        }
        return View(capLogo);
    }

    public async Task<IActionResult> EditCapLogo(int id)
    {
        var capLogo = await _capLogoRepository.GetByIdAsync(id);
        if (capLogo == null) return NotFound();
        return View(capLogo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCapLogo(CapLogo capLogo)
    {
        if (ModelState.IsValid)
        {
            await _capLogoRepository.UpdateAsync(capLogo);
            return RedirectToAction("CapLogos");
        }
        return View(capLogo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCapLogo(int id)
    {
        await _capLogoRepository.DeleteAsync(id);
        return RedirectToAction("CapLogos");
    }

    // Patches Management
    public async Task<IActionResult> Patches()
    {
        var patches = await _patchRepository.GetAllAsync();
        return View(patches);
    }

    public IActionResult CreatePatch()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePatch(Patch patch)
    {
        if (ModelState.IsValid)
        {
            await _patchRepository.CreateAsync(patch);
            return RedirectToAction("Patches");
        }
        return View(patch);
    }

    public async Task<IActionResult> EditPatch(int id)
    {
        var patch = await _patchRepository.GetByIdAsync(id);
        if (patch == null) return NotFound();
        return View(patch);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPatch(Patch patch)
    {
        if (ModelState.IsValid)
        {
            await _patchRepository.UpdateAsync(patch);
            return RedirectToAction("Patches");
        }
        return View(patch);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePatch(int id)
    {
        await _patchRepository.DeleteAsync(id);
        return RedirectToAction("Patches");
    }

    // Jacket Back Management
    public async Task<IActionResult> JacketBacks()
    {
        var jacketBacks = await _jacketBackRepository.GetAllAsync();
        return View(jacketBacks);
    }

    public IActionResult CreateJacketBack()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateJacketBack(JacketBack jacketBack)
    {
        if (ModelState.IsValid)
        {
            await _jacketBackRepository.CreateAsync(jacketBack);
            return RedirectToAction("JacketBacks");
        }
        return View(jacketBack);
    }

    public async Task<IActionResult> EditJacketBack(int id)
    {
        var jacketBack = await _jacketBackRepository.GetByIdAsync(id);
        if (jacketBack == null) return NotFound();
        return View(jacketBack);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditJacketBack(JacketBack jacketBack)
    {
        if (ModelState.IsValid)
        {
            await _jacketBackRepository.UpdateAsync(jacketBack);
            return RedirectToAction("JacketBacks");
        }
        return View(jacketBack);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteJacketBack(int id)
    {
        await _jacketBackRepository.DeleteAsync(id);
        return RedirectToAction("JacketBacks");
    }

    // Chest Logo Management
    public async Task<IActionResult> ChestLogos()
    {
        var chestLogos = await _chestLogoRepository.GetAllAsync();
        return View(chestLogos);
    }

    public IActionResult CreateChestLogo()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateChestLogo(ChestLogo chestLogo)
    {
        if (ModelState.IsValid)
        {
            await _chestLogoRepository.CreateAsync(chestLogo);
            return RedirectToAction("ChestLogos");
        }
        return View(chestLogo);
    }

    public async Task<IActionResult> EditChestLogo(int id)
    {
        var chestLogo = await _chestLogoRepository.GetByIdAsync(id);
        if (chestLogo == null) return NotFound();
        return View(chestLogo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditChestLogo(ChestLogo chestLogo)
    {
        if (ModelState.IsValid)
        {
            await _chestLogoRepository.UpdateAsync(chestLogo);
            return RedirectToAction("ChestLogos");
        }
        return View(chestLogo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteChestLogo(int id)
    {
        await _chestLogoRepository.DeleteAsync(id);
        return RedirectToAction("ChestLogos");
    }

    // Badge Logo Management
    public async Task<IActionResult> BadgeLogos()
    {
        var badgeLogos = await _badgeLogoRepository.GetAllAsync();
        return View(badgeLogos);
    }

    public IActionResult CreateBadgeLogo()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBadgeLogo(BadgeLogo badgeLogo)
    {
        if (ModelState.IsValid)
        {
            await _badgeLogoRepository.CreateAsync(badgeLogo);
            return RedirectToAction("BadgeLogos");
        }
        return View(badgeLogo);
    }

    public async Task<IActionResult> EditBadgeLogo(int id)
    {
        var badgeLogo = await _badgeLogoRepository.GetByIdAsync(id);
        if (badgeLogo == null) return NotFound();
        return View(badgeLogo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBadgeLogo(BadgeLogo badgeLogo)
    {
        if (ModelState.IsValid)
        {
            await _badgeLogoRepository.UpdateAsync(badgeLogo);
            return RedirectToAction("BadgeLogos");
        }
        return View(badgeLogo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBadgeLogo(int id)
    {
        await _badgeLogoRepository.DeleteAsync(id);
        return RedirectToAction("BadgeLogos");
    }

    // Vector Art Management
    public async Task<IActionResult> VectorArts()
    {
        var vectorArts = await _vectorArtRepository.GetAllAsync();
        return View(vectorArts);
    }

    public IActionResult CreateVectorArt()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVectorArt(VectorArt vectorArt)
    {
        if (ModelState.IsValid)
        {
            await _vectorArtRepository.CreateAsync(vectorArt);
            return RedirectToAction("VectorArts");
        }
        return View(vectorArt);
    }

    public async Task<IActionResult> EditVectorArt(int id)
    {
        var vectorArt = await _vectorArtRepository.GetByIdAsync(id);
        if (vectorArt == null) return NotFound();
        return View(vectorArt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditVectorArt(VectorArt vectorArt)
    {
        if (ModelState.IsValid)
        {
            await _vectorArtRepository.UpdateAsync(vectorArt);
            return RedirectToAction("VectorArts");
        }
        return View(vectorArt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVectorArt(int id)
    {
        await _vectorArtRepository.DeleteAsync(id);
        return RedirectToAction("VectorArts");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}

