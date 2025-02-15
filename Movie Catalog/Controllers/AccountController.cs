using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Catalog.Controllers
{
    public class AccountController : Controller
    {
        private readonly MovieCatalogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            MovieCatalogContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View(model);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // First find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName, // Use username instead of email
                        model.Password,
                        isPersistent: true, // Enable remember me functionality
                        lockoutOnFailure: true); // Enable account lockout for security

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Successfully logged in!";
                        return RedirectToAction("Index", "Home");
                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Account is locked out. Please try again later.");
                        return View(model);
                    }
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // Necessary imports for Identity, MVC, and database functionality
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Catalog.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Catalog.Controllers
    {
        public class AccountController : Controller
        {
            // Dependencies required for user management and authentication
            private readonly MovieCatalogContext _context;              // Database context
            private readonly UserManager<ApplicationUser> _userManager;  // Handles user operations
            private readonly SignInManager<ApplicationUser> _signInManager; // Handles authentication

            // Constructor with dependency injection
            public AccountController(
                MovieCatalogContext context,
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager)
            {
                _context = context;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            // Displays the registration form
            public IActionResult Register()
            {
                return View();
            }

            // Handles the registration form submission
            [HttpPost]
            [ValidateAntiForgeryToken] // Prevents cross-site request forgery
            public async Task<IActionResult> Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    // Create new user from the registration data
                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = model.Email
                    };

                    // Attempt to create the user in the database
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Automatically sign in the user after successful registration
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
                        return RedirectToAction("Index", "Home");
                    }

                    // If registration fails, add errors to ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }

            // Displays the login form
            public IActionResult Login()
            {
                return View();
            }

            // Handles the login form submission
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginViewModel model)
            {
                if (ModelState.IsValid)
                {
                    // Find user by email
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        // Attempt to sign in the user
                        var result = await _signInManager.PasswordSignInAsync(
                            user.UserName,
                            model.Password,
                            isPersistent: true, // Enables "Remember me" functionality
                            lockoutOnFailure: true); // Enables account lockout for security

                        if (result.Succeeded)
                        {
                            // Successful login
                            TempData["SuccessMessage"] = "Successfully logged in!";
                            return RedirectToAction("Index", "Home");
                        }
                        if (result.IsLockedOut)
                        {
                            // Handle locked out accounts
                            ModelState.AddModelError("", "Account is locked out. Please try again later.");
                            return View(model);
                        }
                    }

                    // Generic error message for security
                    ModelState.AddModelError("", "Invalid email or password.");
                }

                return View(model);
            }

            // Handles user logout
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
        }
    }

}
}
