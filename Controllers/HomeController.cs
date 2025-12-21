using Microsoft.AspNetCore.Mvc;
using ThreadifyLab.Repositories;

namespace ThreadifyLab.Controllers;

public class HomeController : Controller
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IPortfolioRepository _portfolioRepository;

    public HomeController(IServiceRepository serviceRepository, IPortfolioRepository portfolioRepository)
    {
        _serviceRepository = serviceRepository;
        _portfolioRepository = portfolioRepository;
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

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}

