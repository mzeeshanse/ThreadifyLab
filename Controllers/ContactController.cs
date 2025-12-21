using Microsoft.AspNetCore.Mvc;
using ThreadifyLab.Models;
using ThreadifyLab.Repositories;

namespace ThreadifyLab.Controllers;

public class ContactController : Controller
{
    private readonly IContactRepository _contactRepository;

    public ContactController(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ContactMessage model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Home/Contact.cshtml", model);
        }

        await _contactRepository.CreateAsync(model);
        TempData["SuccessMessage"] = "Thank you for your message! We'll get back to you soon.";
        return RedirectToAction("Contact", "Home");
    }
}

