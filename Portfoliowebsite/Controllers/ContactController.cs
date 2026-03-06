using Microsoft.AspNetCore.Mvc;
using Portfoliowebsite.Models;
using Portfoliowebsite.Services;

namespace Portfoliowebsite.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _email;
        public ContactController(IEmailSender email) => _email = email;

        // GET: Contact
        public IActionResult Index() => View(new ContactViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken] // Bescherming tegen CSRF-aanvallen
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            // Honeypot veld controleren
            if (!string.IsNullOrEmpty(model.Website))
            {
                return BadRequest("Spam detected");
            }
            
            if(model.Name == null || model.Email == null || model.Subject == null || model.Message == null)
            {
                ModelState.AddModelError(string.Empty, "All fields are required.");
                return View(model);
            }
            
            await _email.SendAsync(model.Name, model.Email, model.Subject, model.Message);

            TempData["ThanksName"] = model.Name;
            TempData["ThanksEmail"] = model.Email;
            TempData["ThanksMessage"] = model.Message;

            return RedirectToAction(nameof(Thanks));
        }

        public IActionResult Thanks() => View();
    }
}
