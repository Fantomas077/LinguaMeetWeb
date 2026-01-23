using LinguaMeet.Domain.Entities;
using LinguaMeet.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            FullName = model.Name,
            Email = model.Email,
            UserName = model.Email
        };
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "This Email already exists");
            return View(model);
        }

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            
            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.EmailAdresse);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt");
        return View(model);
    }
    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
    {
        if (!ModelState.IsValid)
            return View(resetPassword);

        var user = await _userManager.FindByEmailAsync(resetPassword.EmailAdresse);

        if (user == null)
        {
            ModelState.AddModelError("", "No account with this email address");
            return View(resetPassword);
        }

        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        
        var result = await _userManager.ResetPasswordAsync(
            user,
            token,
            resetPassword.NewPassword
        );

        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Account");
        }

        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(resetPassword);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
