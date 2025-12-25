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
    private readonly IPricingRepository _pricingRepository;
    private readonly IImageUploadService _imageUploadService;

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
        IVectorArtRepository vectorArtRepository,
        IPricingRepository pricingRepository,
        IImageUploadService imageUploadService)
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
        _pricingRepository = pricingRepository;
        _imageUploadService = imageUploadService;
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
    public async Task<IActionResult> CreatePortfolio(PortfolioItem item, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    item.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(item);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(item);
            }

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
    public async Task<IActionResult> EditPortfolio(PortfolioItem item, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingItem = await _portfolioRepository.GetByIdAsync(item.Id);
                    if (existingItem != null && !string.IsNullOrEmpty(existingItem.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingItem.ImageUrl);
                    }

                    // Upload new image
                    item.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(item);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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
    public async Task<IActionResult> CreateCapLogo(CapLogo capLogo, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    capLogo.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(capLogo);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("imageFile", $"Error uploading image: {ex.Message}");
                    return View(capLogo);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(capLogo);
            }

            try
            {
                await _capLogoRepository.CreateAsync(capLogo);
                return RedirectToAction("CapLogos");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating cap logo: {ex.Message}");
                return View(capLogo);
            }
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
    public async Task<IActionResult> EditCapLogo(CapLogo capLogo, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingCapLogo = await _capLogoRepository.GetByIdAsync(capLogo.Id);
                    if (existingCapLogo != null && !string.IsNullOrEmpty(existingCapLogo.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingCapLogo.ImageUrl);
                    }

                    // Upload new image
                    capLogo.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(capLogo);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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
    public async Task<IActionResult> CreatePatch(Patch patch, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    patch.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(patch);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(patch);
            }

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
    public async Task<IActionResult> EditPatch(Patch patch, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingPatch = await _patchRepository.GetByIdAsync(patch.Id);
                    if (existingPatch != null && !string.IsNullOrEmpty(existingPatch.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingPatch.ImageUrl);
                    }

                    // Upload new image
                    patch.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(patch);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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
    public async Task<IActionResult> CreateJacketBack(JacketBack jacketBack, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    jacketBack.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(jacketBack);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(jacketBack);
            }

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
    public async Task<IActionResult> EditJacketBack(JacketBack jacketBack, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingJacketBack = await _jacketBackRepository.GetByIdAsync(jacketBack.Id);
                    if (existingJacketBack != null && !string.IsNullOrEmpty(existingJacketBack.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingJacketBack.ImageUrl);
                    }

                    // Upload new image
                    jacketBack.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(jacketBack);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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
    public async Task<IActionResult> CreateChestLogo(ChestLogo chestLogo, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    chestLogo.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(chestLogo);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(chestLogo);
            }

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
    public async Task<IActionResult> EditChestLogo(ChestLogo chestLogo, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingChestLogo = await _chestLogoRepository.GetByIdAsync(chestLogo.Id);
                    if (existingChestLogo != null && !string.IsNullOrEmpty(existingChestLogo.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingChestLogo.ImageUrl);
                    }

                    // Upload new image
                    chestLogo.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(chestLogo);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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
    public async Task<IActionResult> CreateBadgeLogo(BadgeLogo badgeLogo, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    badgeLogo.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(badgeLogo);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(badgeLogo);
            }

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
    public async Task<IActionResult> EditBadgeLogo(BadgeLogo badgeLogo, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingBadgeLogo = await _badgeLogoRepository.GetByIdAsync(badgeLogo.Id);
                    if (existingBadgeLogo != null && !string.IsNullOrEmpty(existingBadgeLogo.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingBadgeLogo.ImageUrl);
                    }

                    // Upload new image
                    badgeLogo.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(badgeLogo);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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
    public async Task<IActionResult> CreateVectorArt(VectorArt vectorArt, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    vectorArt.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(vectorArt);
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "Image is required");
                return View(vectorArt);
            }

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
    public async Task<IActionResult> EditVectorArt(VectorArt vectorArt, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    // Delete old image if it exists
                    var existingVectorArt = await _vectorArtRepository.GetByIdAsync(vectorArt.Id);
                    if (existingVectorArt != null && !string.IsNullOrEmpty(existingVectorArt.ImageUrl))
                    {
                        _imageUploadService.DeleteImage(existingVectorArt.ImageUrl);
                    }

                    // Upload new image
                    vectorArt.ImageUrl = await _imageUploadService.UploadImageAsync(imageFile, "images");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("imageFile", ex.Message);
                    return View(vectorArt);
                }
            }
            // If no new image uploaded, ImageUrl from model binding will be used (existing image)

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

    // Pricing Management
    public async Task<IActionResult> Pricings()
    {
        var pricings = await _pricingRepository.GetAllAsync();
        return View(pricings);
    }

    public IActionResult CreatePricing()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePricing(Pricing pricing)
    {
        if (ModelState.IsValid)
        {
            await _pricingRepository.CreateAsync(pricing);
            return RedirectToAction("Pricings");
        }
        return View(pricing);
    }

    public async Task<IActionResult> EditPricing(int id)
    {
        var pricing = await _pricingRepository.GetByIdAsync(id);
        if (pricing == null) return NotFound();
        return View(pricing);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPricing(Pricing pricing)
    {
        if (ModelState.IsValid)
        {
            await _pricingRepository.UpdateAsync(pricing);
            return RedirectToAction("Pricings");
        }
        return View(pricing);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePricing(int id)
    {
        await _pricingRepository.DeleteAsync(id);
        return RedirectToAction("Pricings");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}

