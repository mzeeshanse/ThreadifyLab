using Microsoft.AspNetCore.Mvc;
using ThreadifyLab.Repositories;
using ThreadifyLab.Models;

namespace ThreadifyLab.Controllers;

public class HomeController : Controller
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly ICapLogoRepository _capLogoRepository;
    private readonly IPatchRepository _patchRepository;
    private readonly IJacketBackRepository _jacketBackRepository;
    private readonly IChestLogoRepository _chestLogoRepository;
    private readonly IBadgeLogoRepository _badgeLogoRepository;
    private readonly IVectorArtRepository _vectorArtRepository;

    public HomeController(
        IServiceRepository serviceRepository,
        IPortfolioRepository portfolioRepository,
        ICapLogoRepository capLogoRepository,
        IPatchRepository patchRepository,
        IJacketBackRepository jacketBackRepository,
        IChestLogoRepository chestLogoRepository,
        IBadgeLogoRepository badgeLogoRepository,
        IVectorArtRepository vectorArtRepository)
    {
        _serviceRepository = serviceRepository;
        _portfolioRepository = portfolioRepository;
        _capLogoRepository = capLogoRepository;
        _patchRepository = patchRepository;
        _jacketBackRepository = jacketBackRepository;
        _chestLogoRepository = chestLogoRepository;
        _badgeLogoRepository = badgeLogoRepository;
        _vectorArtRepository = vectorArtRepository;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Services = await _serviceRepository.GetAllActiveAsync();
        ViewBag.Portfolio = await _portfolioRepository.GetAllActiveAsync();
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public async Task<IActionResult> Services()
    {
        var services = await _serviceRepository.GetAllActiveAsync();
        return View(services);
    }

    public async Task<IActionResult> Portfolio()
    {
        var portfolio = await _portfolioRepository.GetAllActiveAsync();
        return View(portfolio);
    }

    public async Task<IActionResult> CapLogo()
    {
        var capLogos = await _capLogoRepository.GetAllActiveAsync();
        return View(capLogos);
    }

    public IActionResult CapLogoOld()
    {
        // Sample cap logo data - you can replace this with database data later
        var capLogos = new List<CapLogo>
        {
            new CapLogo
            {
                Id = 1,
                Title = "Classic Cap Logo",
                Description = "Traditional cap logo design perfect for sports teams and corporate branding. Clean and professional appearance.",
                ImageUrl = "https://via.placeholder.com/400x300/6366f1/ffffff?text=Classic+Cap+Logo",
                DisplayOrder = 1,
                IsActive = true
            },
            new CapLogo
            {
                Id = 2,
                Title = "3D Puff Cap Logo",
                Description = "Dimensional 3D puff embroidery that stands out with depth and texture. Ideal for premium branding.",
                ImageUrl = "https://via.placeholder.com/400x300/8b5cf6/ffffff?text=3D+Puff+Cap",
                DisplayOrder = 2,
                IsActive = true
            },
            new CapLogo
            {
                Id = 3,
                Title = "Text-Based Cap Logo",
                Description = "Bold text-based designs optimized for cap embroidery. Clear and readable from a distance.",
                ImageUrl = "https://via.placeholder.com/400x300/ec4899/ffffff?text=Text+Cap+Logo",
                DisplayOrder = 3,
                IsActive = true
            },
            new CapLogo
            {
                Id = 4,
                Title = "Minimalist Cap Logo",
                Description = "Simple and elegant minimalist designs that work beautifully on caps. Modern and sophisticated.",
                ImageUrl = "https://via.placeholder.com/400x300/10b981/ffffff?text=Minimalist+Cap",
                DisplayOrder = 4,
                IsActive = true
            },
            new CapLogo
            {
                Id = 5,
                Title = "Sports Team Cap Logo",
                Description = "Dynamic sports team logos designed for caps. Perfect for team uniforms and fan merchandise.",
                ImageUrl = "https://via.placeholder.com/400x300/f59e0b/ffffff?text=Sports+Team+Cap",
                DisplayOrder = 5,
                IsActive = true
            },
            new CapLogo
            {
                Id = 6,
                Title = "Corporate Cap Logo",
                Description = "Professional corporate logos optimized for cap embroidery. Maintains brand identity on curved surfaces.",
                ImageUrl = "https://via.placeholder.com/400x300/ef4444/ffffff?text=Corporate+Cap",
                DisplayOrder = 6,
                IsActive = true
            }
        };

        return View(capLogos);
    }

    public async Task<IActionResult> Patches()
    {
        var patches = await _patchRepository.GetAllActiveAsync();
        return View(patches);
    }

    public IActionResult PatchesOld()
    {
        var patches = new List<Patch>
        {
            new Patch
            {
                Id = 1,
                Title = "Embroidered Patches",
                Description = "High-quality embroidered patches perfect for uniforms, jackets, and accessories. Durable and professional finish.",
                ImageUrl = "https://via.placeholder.com/400x300/6366f1/ffffff?text=Embroidered+Patches",
                DisplayOrder = 1,
                IsActive = true
            },
            new Patch
            {
                Id = 2,
                Title = "Custom Shape Patches",
                Description = "Custom-shaped patches designed to match your unique brand identity. Available in various shapes and sizes.",
                ImageUrl = "https://via.placeholder.com/400x300/8b5cf6/ffffff?text=Custom+Shape+Patches",
                DisplayOrder = 2,
                IsActive = true
            },
            new Patch
            {
                Id = 3,
                Title = "Military Style Patches",
                Description = "Authentic military-style patches with detailed embroidery. Perfect for organizations and teams.",
                ImageUrl = "https://via.placeholder.com/400x300/ec4899/ffffff?text=Military+Style",
                DisplayOrder = 3,
                IsActive = true
            },
            new Patch
            {
                Id = 4,
                Title = "3D Puff Patches",
                Description = "Dimensional 3D puff patches that create a raised, textured effect. Eye-catching and premium quality.",
                ImageUrl = "https://via.placeholder.com/400x300/10b981/ffffff?text=3D+Puff+Patches",
                DisplayOrder = 4,
                IsActive = true
            },
            new Patch
            {
                Id = 5,
                Title = "Logo Patches",
                Description = "Professional logo patches for brand promotion. Ideal for corporate uniforms and promotional items.",
                ImageUrl = "https://via.placeholder.com/400x300/f59e0b/ffffff?text=Logo+Patches",
                DisplayOrder = 5,
                IsActive = true
            },
            new Patch
            {
                Id = 6,
                Title = "Text Patches",
                Description = "Custom text patches with various font styles. Perfect for names, titles, and identification purposes.",
                ImageUrl = "https://via.placeholder.com/400x300/ef4444/ffffff?text=Text+Patches",
                DisplayOrder = 6,
                IsActive = true
            }
        };

        return View(patches);
    }

    public async Task<IActionResult> JacketBack()
    {
        var jacketBacks = await _jacketBackRepository.GetAllActiveAsync();
        return View(jacketBacks);
    }

    public IActionResult JacketBackOld()
    {
        var jacketBacks = new List<JacketBack>
        {
            new JacketBack
            {
                Id = 1,
                Title = "Large Back Logo",
                Description = "Bold and prominent jacket back logos that make a strong statement. Perfect for team jackets and outerwear.",
                ImageUrl = "https://via.placeholder.com/400x300/6366f1/ffffff?text=Large+Back+Logo",
                DisplayOrder = 1,
                IsActive = true
            },
            new JacketBack
            {
                Id = 2,
                Title = "Text-Based Back Design",
                Description = "Text-focused designs for jacket backs. Clear and readable from a distance with professional typography.",
                ImageUrl = "https://via.placeholder.com/400x300/8b5cf6/ffffff?text=Text+Back+Design",
                DisplayOrder = 2,
                IsActive = true
            },
            new JacketBack
            {
                Id = 3,
                Title = "Full Back Embroidered Design",
                Description = "Complete back coverage with detailed embroidery. Ideal for premium jackets and custom outerwear.",
                ImageUrl = "https://via.placeholder.com/400x300/ec4899/ffffff?text=Full+Back+Design",
                DisplayOrder = 3,
                IsActive = true
            },
            new JacketBack
            {
                Id = 4,
                Title = "Sports Team Back Logo",
                Description = "Dynamic sports team logos optimized for jacket backs. Perfect for team uniforms and fan gear.",
                ImageUrl = "https://via.placeholder.com/400x300/10b981/ffffff?text=Sports+Team+Back",
                DisplayOrder = 4,
                IsActive = true
            },
            new JacketBack
            {
                Id = 5,
                Title = "Corporate Back Logo",
                Description = "Professional corporate logos for jacket backs. Maintains brand visibility and professionalism.",
                ImageUrl = "https://via.placeholder.com/400x300/f59e0b/ffffff?text=Corporate+Back+Logo",
                DisplayOrder = 5,
                IsActive = true
            },
            new JacketBack
            {
                Id = 6,
                Title = "Artistic Back Design",
                Description = "Creative and artistic designs for jacket backs. Unique patterns and custom artwork options.",
                ImageUrl = "https://via.placeholder.com/400x300/ef4444/ffffff?text=Artistic+Back+Design",
                DisplayOrder = 6,
                IsActive = true
            }
        };

        return View(jacketBacks);
    }

    public async Task<IActionResult> ChestLogo()
    {
        var chestLogos = await _chestLogoRepository.GetAllActiveAsync();
        return View(chestLogos);
    }

    public IActionResult ChestLogoOld()
    {
        var chestLogos = new List<ChestLogo>
        {
            new ChestLogo
            {
                Id = 1,
                Title = "Left Chest Logo",
                Description = "Classic left chest logo placement. Professional and traditional positioning for uniforms and polos.",
                ImageUrl = "https://via.placeholder.com/400x300/6366f1/ffffff?text=Left+Chest+Logo",
                DisplayOrder = 1,
                IsActive = true
            },
            new ChestLogo
            {
                Id = 2,
                Title = "Right Chest Logo",
                Description = "Right chest logo placement for balanced design. Perfect for dual-logo applications.",
                ImageUrl = "https://via.placeholder.com/400x300/8b5cf6/ffffff?text=Right+Chest+Logo",
                DisplayOrder = 2,
                IsActive = true
            },
            new ChestLogo
            {
                Id = 3,
                Title = "Center Chest Logo",
                Description = "Centered chest logo for maximum visibility. Ideal for promotional and branded apparel.",
                ImageUrl = "https://via.placeholder.com/400x300/ec4899/ffffff?text=Center+Chest+Logo",
                DisplayOrder = 3,
                IsActive = true
            },
            new ChestLogo
            {
                Id = 4,
                Title = "Small Chest Logo",
                Description = "Compact chest logos for subtle branding. Perfect for professional business attire.",
                ImageUrl = "https://via.placeholder.com/400x300/10b981/ffffff?text=Small+Chest+Logo",
                DisplayOrder = 4,
                IsActive = true
            },
            new ChestLogo
            {
                Id = 5,
                Title = "Corporate Chest Logo",
                Description = "Professional corporate chest logos. Clean and professional appearance for business uniforms.",
                ImageUrl = "https://via.placeholder.com/400x300/f59e0b/ffffff?text=Corporate+Chest+Logo",
                DisplayOrder = 5,
                IsActive = true
            },
            new ChestLogo
            {
                Id = 6,
                Title = "3D Puff Chest Logo",
                Description = "Dimensional 3D puff chest logos with raised texture. Premium look and feel for high-end apparel.",
                ImageUrl = "https://via.placeholder.com/400x300/ef4444/ffffff?text=3D+Puff+Chest+Logo",
                DisplayOrder = 6,
                IsActive = true
            }
        };

        return View(chestLogos);
    }

    public async Task<IActionResult> BadgeLogo()
    {
        var badgeLogos = await _badgeLogoRepository.GetAllActiveAsync();
        return View(badgeLogos);
    }

    public IActionResult BadgeLogoOld()
    {
        var badgeLogos = new List<BadgeLogo>
        {
            new BadgeLogo
            {
                Id = 1,
                Title = "Circular Badge Logo",
                Description = "Classic circular badge designs. Traditional and professional appearance for uniforms and identification.",
                ImageUrl = "https://via.placeholder.com/400x300/6366f1/ffffff?text=Circular+Badge",
                DisplayOrder = 1,
                IsActive = true
            },
            new BadgeLogo
            {
                Id = 2,
                Title = "Shield Badge Logo",
                Description = "Shield-shaped badge logos with detailed embroidery. Perfect for security, law enforcement, and organizations.",
                ImageUrl = "https://via.placeholder.com/400x300/8b5cf6/ffffff?text=Shield+Badge",
                DisplayOrder = 2,
                IsActive = true
            },
            new BadgeLogo
            {
                Id = 3,
                Title = "Custom Shape Badge",
                Description = "Custom-shaped badge logos designed to your specifications. Unique designs for special applications.",
                ImageUrl = "https://via.placeholder.com/400x300/ec4899/ffffff?text=Custom+Badge",
                DisplayOrder = 3,
                IsActive = true
            },
            new BadgeLogo
            {
                Id = 4,
                Title = "Name Badge Logo",
                Description = "Personalized name badge logos. Ideal for identification, events, and professional settings.",
                ImageUrl = "https://via.placeholder.com/400x300/10b981/ffffff?text=Name+Badge",
                DisplayOrder = 4,
                IsActive = true
            },
            new BadgeLogo
            {
                Id = 5,
                Title = "Rank Badge Logo",
                Description = "Rank and title badge logos. Perfect for military, security, and organizational hierarchy displays.",
                ImageUrl = "https://via.placeholder.com/400x300/f59e0b/ffffff?text=Rank+Badge",
                DisplayOrder = 5,
                IsActive = true
            },
            new BadgeLogo
            {
                Id = 6,
                Title = "Decorative Badge Logo",
                Description = "Ornate decorative badge logos with intricate details. Premium quality for special occasions.",
                ImageUrl = "https://via.placeholder.com/400x300/ef4444/ffffff?text=Decorative+Badge",
                DisplayOrder = 6,
                IsActive = true
            }
        };

        return View(badgeLogos);
    }

    public async Task<IActionResult> VectorArt()
    {
        var vectorArts = await _vectorArtRepository.GetAllActiveAsync();
        return View(vectorArts);
    }

    public IActionResult VectorArtOld()
    {
        var vectorArts = new List<VectorArt>
        {
            new VectorArt
            {
                Id = 1,
                Title = "Logo Vectorization",
                Description = "Convert raster images to clean, scalable vector graphics. Perfect for logos, icons, and brand assets that need to scale to any size.",
                ImageUrl = "https://via.placeholder.com/400x300/6366f1/ffffff?text=Logo+Vectorization",
                DisplayOrder = 1,
                IsActive = true
            },
            new VectorArt
            {
                Id = 2,
                Title = "Illustration Vector Art",
                Description = "Professional vector illustrations created from scratch or converted from existing artwork. High-quality scalable graphics for any application.",
                ImageUrl = "https://via.placeholder.com/400x300/8b5cf6/ffffff?text=Illustration+Vector",
                DisplayOrder = 2,
                IsActive = true
            },
            new VectorArt
            {
                Id = 3,
                Title = "Text to Vector",
                Description = "Convert text designs into clean vector format. Perfect for typography, lettering, and custom text-based graphics.",
                ImageUrl = "https://via.placeholder.com/400x300/ec4899/ffffff?text=Text+to+Vector",
                DisplayOrder = 3,
                IsActive = true
            },
            new VectorArt
            {
                Id = 4,
                Title = "Icon Vector Art",
                Description = "Custom icon sets and individual icons in vector format. Scalable and crisp at any size for web, print, and digital applications.",
                ImageUrl = "https://via.placeholder.com/400x300/10b981/ffffff?text=Icon+Vector+Art",
                DisplayOrder = 4,
                IsActive = true
            },
            new VectorArt
            {
                Id = 5,
                Title = "Complex Vector Graphics",
                Description = "Detailed and complex vector graphics with multiple layers and elements. Professional quality for advanced design projects.",
                ImageUrl = "https://via.placeholder.com/400x300/f59e0b/ffffff?text=Complex+Vector",
                DisplayOrder = 5,
                IsActive = true
            },
            new VectorArt
            {
                Id = 6,
                Title = "Vector Art Cleanup",
                Description = "Clean and optimize existing vector files. Remove unnecessary points, fix paths, and optimize for better performance.",
                ImageUrl = "https://via.placeholder.com/400x300/ef4444/ffffff?text=Vector+Cleanup",
                DisplayOrder = 6,
                IsActive = true
            }
        };

        return View(vectorArts);
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Pricing()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}

