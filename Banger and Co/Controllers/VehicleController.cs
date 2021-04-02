using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace Banger_and_Co.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly DataContext _db;
        private readonly UserService _userService;
        private readonly BookingService _bookingService;
        private readonly VehicleService _vehicleService;
        private readonly EquipmentService _equipmentService;
        private readonly ManufacturerService _manufacturerService;
        private readonly FileUploadService _fileUploader;

        public VehicleController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, DataContext db, UserService userService, BookingService bookingService, VehicleService vehicleService, EquipmentService equipmentService, ManufacturerService manufacturerService, FileUploadService fileUploader)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
            _userService = userService;
            _bookingService = bookingService;
            _vehicleService = vehicleService;
            _equipmentService = equipmentService;
            _manufacturerService = manufacturerService;
            _fileUploader = fileUploader;
        }

        
    }
}
