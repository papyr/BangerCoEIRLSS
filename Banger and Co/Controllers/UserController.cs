using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace Banger_and_Co.Controllers
{
    public class UserController:Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly DataContext _db;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly FileUploadService _fileUploader;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, DataContext db, IHostingEnvironment hostingEnv, FileUploadService fileUploader)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
            _hostingEnv = hostingEnv;
            _fileUploader = fileUploader;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Verify()
        {
            return View();
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public async Task<IActionResult> SignIn([FromUri] string email, string password)
        {
            if(User.Identity.Name != null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);  //Get associated roles
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    if (role[0].Equals("Client"))
                    {
                        return Json("Home");
                    }
                    else if (role[0].Equals("Admin") || role[0].Equals("SuperUser"))
                    {
                        return Json("Admin");
                    }
                }
            }

            return Json("Please enter valid login credentials");
        }

        [System.Web.Http.HttpPost]
        public async Task<IActionResult> Join([FromUri] string name, string email, string phone, string pwd, DateTime dob)
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            string username = Regex.Replace(name, @"\s+", "");

            User user = new User
            {
                FullName = name,
                Role = "Client",
                UserName = username,
                Email = email,
                PhoneNumber = phone,
                DateOfBirth = dob,
                Status = "Unverified"
            };

            var result = await _userManager.CreateAsync(user, pwd);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                return Json("success");
            }

            return Json("failure");
        }

        [System.Web.Http.HttpPost]
        public async Task<IActionResult> UploadVerfication(IFormFile licenseimg, IFormFile idimg, string useremail)
        {
            User user = await _userManager.FindByEmailAsync(useremail);
            string licenseFileName = user.UserName + "license";
            string addFileName = user.UserName + "additional";
            string location = "/User Document Images";
            string licenseDbName = null;
            string additionalDbName = null;

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Join", "Home");
            }

            licenseDbName = _fileUploader.UploadFileAsync(licenseimg, licenseFileName, location).Result;
            additionalDbName = _fileUploader.UploadFileAsync(licenseimg, addFileName, location).Result;

            //string root = _hostingEnv.WebRootPath;

            //string licenseExt = Path.GetExtension(licenseimg.FileName);
            //string additionalExt = Path.GetExtension(idimg.FileName);

            //string licenseFileName = user.UserName + "license";
            //string additonalFileName = user.UserName + "additional";

            //string licenseDbName = licenseFileName = licenseFileName + DateTime.Now.ToString("yymmssfff") + licenseExt;
            //string additionalDbName = additonalFileName = additonalFileName + DateTime.Now.ToString("yymmssfff") + additionalExt;

            //string licensePath = Path.Combine(root + "/User Document Images", licenseDbName);
            //string additionalPath = Path.Combine(root + "/User Document Images", additionalDbName);

            //using (var filestream = new FileStream(licensePath, FileMode.Create))
            //{
            //    await licenseimg.CopyToAsync(filestream);
            //}

            //using (var filestream = new FileStream(additionalPath, FileMode.Create))
            //{
            //    await idimg.CopyToAsync(filestream);
            //}

            user.License = licenseDbName;
            user.AdditonalIdentification = additionalDbName;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("SignIn", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [System.Web.Http.HttpPost]
        public IActionResult CheckEmailPhone([FromUri] string email, string phone)
        {
            var emailCheck = _db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
            var phoneCheck = _db.Users.Where(u => u.PhoneNumber.Equals(phone)).FirstOrDefault();

            if (phoneCheck != null && emailCheck != null)
            {
                return Json("notavailableemailphone");
            }
            else if (emailCheck != null)
            {
                return Json("notavailableemail");
            }
            else if (phoneCheck != null)
            {
                return Json("notavailablephone");
            }
            else
            {
                return Json("available");
            }
        }

        public IActionResult RedirectToLogin()
        {
            TempData["ErrorMessage"] = "Login to make a booking";
            return RedirectToAction("SignIn","Home");
        }
    }
}
