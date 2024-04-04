using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class UserController : Controller
{
    //private readonly UserManager<IdentityUser> _userManager;
    //private readonly SignInManager<IdentityUser> _signInManager;
    //private readonly RoleManager<IdentityRole> _roleManager;

    //public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    //{
    //    _userManager = userManager;
    //    _signInManager = signInManager;
    //    _roleManager = roleManager;
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Register(string email, string password)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var user = new IdentityUser { UserName = email, Email = email };
    //        var result = await _userManager.CreateAsync(user, password);

    //        if (result.Succeeded)
    //        {
    //            // Check if the "Client" role exists, if not, create it
    //            if (!await _roleManager.RoleExistsAsync("Client"))
    //            {
    //                await _roleManager.CreateAsync(new IdentityRole("Client"));
    //            }

    //            // Assign the "Client" role to the new user
    //            await _userManager.AddToRoleAsync(user, "Client");

    //            // You can optionally sign in the user here if needed
    //            await _signInManager.SignInAsync(user, isPersistent: false);

    //            return RedirectToAction("Index", "Home");
    //        }
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.AddModelError(string.Empty, error.Description);
    //        }
    //    }

    //    // If we got this far, something failed, redisplay form
    //    return View();
    //}
}
