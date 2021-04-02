using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Banger_and_Co.Models;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Models;

namespace Banger_and_Co.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "0123456789";
        const string SPECIALS = @"@#";
        const string superAdminEmail = "admin@bangerandco.com";

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewModel myModel = new ViewModel();

            string[] roles = { "SuperUser", "Admin", "Client" };

            foreach (var role in roles)
            {
                var roleStatus = await _roleManager.RoleExistsAsync(role);
                if (!roleStatus)
                {
                    await _roleManager.CreateAsync(new UserRole { Name = role });
                }
            }

            var superUser = await _userManager.FindByEmailAsync(superAdminEmail);
            if (superUser == null)
            {
                User super = new User
                {
                    UserName = "SuperUser",
                    Role = "SuperUser",
                    Email = superAdminEmail,
                    Status = "Verified"
                };

                string password = GeneratePassword(true, true, true, false, 10);

                var adminResult = await _userManager.CreateAsync(super, password);

                if (adminResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(super, "SuperUser");
                }
            }

            if (User.Identity.Name != null)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                var role = await _userManager.GetRolesAsync(user);

                if (role[0].Equals("SuperUser"))
                {
                    myModel.Admin = true;
                }
                else if(role[0].Equals("Admin"))
                {
                    myModel.Admin = true;
                }
            }

            return View(myModel);
        }

        public async Task<IActionResult> SignIn()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            ViewModel model = new ViewModel();

            var errMsg = TempData["ErrorMessage"] as string;

            if (errMsg != null)
            {
                model.ErrorMessage = errMsg;
            }

            model.HttpContext = HttpContext;
            model.UserManager = _userManager;

            return View(model);
        }

        public async Task<IActionResult> Join()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            ViewModel model = new ViewModel();

            var errMsg = TempData["ErrorMessage"] as string;

            if (errMsg != null)
            {
                model.ErrorMessage = errMsg;
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial, int passwordSize)
        {
            char[] _password = new char[passwordSize];
            string charSet = "";
            System.Random _random = new Random();
            int counter;

            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CASE;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
    }
}
