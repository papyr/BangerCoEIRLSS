using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banger_and_Co.Controllers
{
    public class BookingController:Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly DataContext _db;

        public BookingController(ILogger<BookingController> logger, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, DataContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MakeBooking()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableVehicles()
        {
            return View();
        }
    }
}
