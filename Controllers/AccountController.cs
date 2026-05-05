using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View("login");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string email, string password, string role)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);

                if (role == "Docent")
                {
                    var docent = new Docent { Naam = email, IdentityUserId = user.Id };
                    _context.Docenten.Add(docent);
                }
                else if (role == "Leerling")
                {
                    var leerling = new Leerling { Naam = email, IdentityUserId = user.Id };
                    _context.Leerlingen.Add(leerling);
                }

                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description;
        }
        return View("login");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View("login");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Inloggegevens zijn onjuist.";
        }
        return View("login");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}