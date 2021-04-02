using Banger_and_Co.Models;
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
    public class AdminController:Controller
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
        private readonly FileUploadService _fileUploader;
        private readonly ManufacturerService _manufacturerService;

        public AdminController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<UserRole> roleManager, DataContext db, UserService userService, BookingService bookingService, VehicleService vehicleService, EquipmentService equipmentService, FileUploadService fileUploader, ManufacturerService manufacturerService)
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
            _fileUploader = fileUploader;
            _manufacturerService = manufacturerService;
        }

        public async Task<IActionResult> Index()
        {
            ViewModel myModel = new ViewModel();

            if (User.Identity.Name != null)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                var role = await _userManager.GetRolesAsync(user);

                if (!role[0].Equals("SuperUser") && !role[0].Equals("Admin"))
                {
                    return RedirectToAction("AccessDenied", "User");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "User");
            }

            myModel.NumberOfUsers = _userService.GetAdminUserCount();
            myModel.NumberOfBookings = _bookingService.GetCount();
            myModel.NumberOfVehicles = _vehicleService.GetCount();
            myModel.NumberOfEquipment = _equipmentService.GetCount();
            myModel.Users = _userService.Get10(0);
            myModel.Bookings = _bookingService.Get10(0);
            myModel.Vehicles = _vehicleService.Get10(0);
            myModel.Equipments = _equipmentService.Get10(0);
            myModel.UserManager = _userManager;
            myModel.BookingService = _bookingService;
            myModel.Admin = true;

            return View(myModel);
        }

        //USER FUNCTIONS
        [System.Web.Http.HttpPost]
        public JsonResult GetUsers([FromUri] int skip, string keyword, string role, string status)
        {
            List<string> html = new List<string>();
            List<User> users = new List<User>();
            int count = 0;

            if(keyword != null)
            {
                if(!keyword.Equals(""))
                {
                    if (_userService.SearchEmailGetCount(keyword) != 0)
                    {
                        count = _userService.SearchEmailGetCount(keyword);
                        users = _userService.SearchEmailGet10(keyword, skip);
                    }
                    else if (_userService.SearchIdGetCount(keyword) != 0)
                    {
                        count = _userService.SearchIdGetCount(keyword);
                        users = _userService.SearchIdGet10(keyword, skip);
                    }
                    else if (_userService.SearchNameGetCount(keyword) != 0)
                    {
                        count = _userService.SearchNameGetCount(keyword);
                        users = _userService.SearchNameGet10(keyword, skip);
                    }
                    else if (_userService.SearchRoleGetCount(keyword) != 0)
                    {
                        count = _userService.SearchRoleGetCount(keyword);
                        users = _userService.SearchRoleGet10(keyword, skip);
                    }
                    else if (_userService.SearchStatusGetCount(keyword) != 0)
                    {
                        count = _userService.SearchStatusGetCount(keyword);
                        users = _userService.SearchStatusGet10(keyword, skip);
                    }
                }
                else
                {
                    count = _userService.GetCount();
                    if (skip == 0)
                    {
                        users = _userService.Get10(0);
                    }
                    else
                    {
                        users = _userService.Get10(skip);
                    }
                }
            }
            else if(role != null)
            {
                if (status != null)
                {
                    count = _userService.SearchRoleStatusGetCount(role, status);
                    if (skip == 0)
                    {
                        users = _userService.SearchRoleStatusGet10(role, status, 0);
                    }
                    else
                    {
                        users = _userService.SearchRoleStatusGet10(role, status, skip);
                    }
                }
                else
                {
                    count = _userService.SearchRoleGetCount(role);
                    if (skip == 0)
                    {
                        users = _userService.SearchRoleGet10(role, 0);
                    }
                    else
                    {
                        users = _userService.SearchRoleGet10(role, skip);
                    }
                }
            }
            else if(status != null)
            {
                if (role != null)
                {
                    count = _userService.SearchRoleStatusGetCount(role, status);
                    if (skip == 0)
                    {
                        users = _userService.SearchRoleStatusGet10(role, status, 0);
                    }
                    else
                    {
                        users = _userService.SearchRoleStatusGet10(role, status, skip);
                    }
                }
                else
                {
                    count = _userService.SearchStatusGetCount(status);
                    if (skip == 0)
                    {
                        users = _userService.SearchStatusGet10(status, 0);
                    }
                    else
                    {
                        users = _userService.SearchStatusGet10(status, skip);
                    }
                }
            }
            else
            {
                count = _userService.GetCount();
                if (skip == 0)
                {
                    users = _userService.Get10(0);
                }
                else
                {
                    users = _userService.Get10(skip);
                }
            }

            if (users.Count != 0)
            {
                if(skip == 0)
                {
                    html.Add(count.ToString());
                }

                foreach (User user in users)
                {
                    int index = users.IndexOf(user);

                    string code = "<div class='admin_dashboard_content_users_users_user " + index + "'>"
                            + "<div class='admin_dashboard_content_users_users_user_status'>";

                    if (user.Status.Equals("Unverified"))
                    {
                        code += "<span class='admin_dashboard_content_users_users_user_status_notverified'>NOT VERIFIED</span>";
                    }
                    else
                    {
                        if (user.Status.Equals("Blacklisted"))
                        {
                            if (_bookingService.GetCountOfUser(user.Id) != 0)
                            {
                                code += "<span class='admin_dashboard_content_users_users_user_status_blacklisted'>BLACKLISTED<svg onclick='removeBlacklist(" + user.Id + "," + index + ")' id='userremoveblacklist' xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z' /></svg></span>";
                                code += "<span class='admin_dashboard_content_users_users_user_status_repeat'>REPEAT</span>";
                                code += "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                            }
                            else
                            {
                                code += "<span class='admin_dashboard_content_users_users_user_status_blacklisted'>BLACKLISTED<svg onclick='removeBlacklist(" + user.Id + "," + index + ")' id='userremoveblacklist' xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z' /></svg></span>";
                                code += "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                            }
                        }
                        else
                        {
                            if (_bookingService.GetCountOfUser(user.Id) != 0)
                            {
                                code += "<span class='admin_dashboard_content_users_users_user_status_repeat'>REPEAT</span>";
                                code += "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                            }
                            else
                            {
                                code += "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                            }
                        }
                    }

                    code += "</div><div class='admin_dashboard_content_users_users_user_role'>";

                    var currentRole = _userManager.GetRolesAsync(_userManager.FindByNameAsync(User.Identity.Name).Result).Result[0];

                    code += "<label>" + user.Role + "</label>";
                    
                    if(currentRole.Equals("SuperUser"))
                    {
                        if(user.Role.Equals("Client"))
                        {
                            code += "<button id='usermakeadmin' onclick='toggleAdminPriviledge(" + user.Id + "," + index + ")'>Grant Admin Privileges</button>";
                        }
                        else
                        {
                            code += "<button id='usermakeadmin' onclick='toggleAdminPriviledge(" + user.Id + "," + index + ")'>Revoke Admin Privileges</button>";
                        }
                    }

                    string code3 = "</div>"
                    + "<label>USER #" + user.Id + "</label>"
                    + "<label>Name: <span>" + user.FullName + "</span></label>"
                    + "<label>E-mail Address: <span>" + user.Email + "</span></label>"
                    + "<label>Contact Number: <span>" + user.PhoneNumber + "</span></label>"
                    + "<div class='admin_dashboard_content_users_users_user_buttons'>"
                    + "<div>"
                    //+ "<button id='usermoreinfo' onclick='userEdit(" + user.Id + ", 0," + index + ")'>More Info</button><button onclick='userEdit(" + user.Id + ", 1)' id='useredit'>Edit</button></div><div>"
                    + "<button id='usermoreinfo' onclick='userEdit(" + user.Id + ", 0," + index + ")'>More Info</button></div><div>"
                    + "<button id='userremove' onclick='removeUser(" + user.Id + ")'>Remove User<svg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M9 19c0 .552-.448 1-1 1s-1-.448-1-1v-10c0-.552.448-1 1-1s1 .448 1 1v10zm4 0c0 .552-.448 1-1 1s-1-.448-1-1v-10c0-.552.448-1 1-1s1 .448 1 1v10zm4 0c0 .552-.448 1-1 1s-1-.448-1-1v-10c0-.552.448-1 1-1s1 .448 1 1v10zm5-17v2h-20v-2h5.711c.9 0 1.631-1.099 1.631-2h5.315c0 .901.73 2 1.631 2h5.712zm-3 4v16h-14v-16h-2v18h18v-18h-2z'/></svg></button>"
                    + "</div></div></div>";

                    code += code3;

                    html.Add(code);
                }
            }

            return Json(html);
        }

        [System.Web.Http.HttpPost]
        public JsonResult GetEditUserDetails([FromUri] int id, int index)
        {
            String html = null;

            String currentRole =_userManager.GetRolesAsync(_userManager.FindByNameAsync(User.Identity.Name).Result).Result[0];

            User user = _userManager.FindByIdAsync(id.ToString()).Result;

            html += "<div id='userdetailsoverlay' class='admin_dashboard_content_users_popup_overlay " + index + "'>"
                + "<button class='admin_dashboard_content_users_popup_overlay_close' onclick='closeUserOverlay()' id='closepopup'><svg xmlns='http://www.w3.org/2000/svg' width='20' height='20' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z'/></svg></button>";

            html += "<div class='admin_dashboard_content_users_users_user_status'>";

            if (user.Status.Equals("Unverified"))
            {
                html += "<span class='admin_dashboard_content_users_users_user_status_notverified'>NOT VERIFIED</span>";
            }
            else
            {
                if (user.Status.Equals("Blacklisted"))
                {
                    if (_bookingService.GetCountOfUser(user.Id) != 0)
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_blacklisted'>BLACKLISTED<svg onclick='removeBlacklist(" + user.Id + "," + index + ")' id='userremoveblacklist' xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z' /><label id='userremoveblacklistid' hidden>" + user.Id + "</label></svg></span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_repeat'>REPEAT</span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                    else
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_blacklisted'>BLACKLISTED<svg onclick='removeBlacklist(" + user.Id + "," + index + ")' id='userremoveblacklist' xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z' /><label id='userremoveblacklistid' hidden>" + user.Id + "</label></svg></span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                }
                else
                {
                    if (_bookingService.GetCountOfUser(user.Id) != 0)
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_repeat'>REPEAT</span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                    else
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                }
            }

            html += "</div>";

            html += "<div class='admin_dashboard_content_users_popup_overlay_content'>";

            html += "<div class='admin_dashboard_content_users_popup_overlay_content_role'>"
                + "<label>Type: " + user.Role + "</label>";

            if (currentRole.Equals("SuperUser"))
            {
                if (user.Role.Equals("Client"))
                {
                    html += "<button id='usermakeadmin' onclick='toggleAdminPriviledge(" + user.Id + "," + index + ")'>Grant Admin Privileges</button>";
                }
                else
                {
                    html += "<button id='usermakeclient' onclick='toggleAdminPriviledge(" + user.Id + "," + index + ")'>Revoke Admin Privileges</button>";
                }
            }

            html += "</div>";

            html += "<div class='admin_dashboard_content_users_popup_overlay_content_info'>"
                + "<div class='admin_dashboard_content_users_popup_overlay_content_info_details'>"
                + "<div><label>USER #" + user.Id.ToString() + "</label></div>"
                + "<div><label>Name: </label><label hidden id='usereditnamehid'>" + user.FullName + "</label><input type='text' id='usereditname' pattern='^[A-Za-z ,.'-]+$' value='" + user.FullName + "'></div>"
                + "<div><label>E-mail Address: </label><label hidden id='usereditemailhid'>" + user.Email + "</label><input type='email' id='usereditemail' value='" + user.Email + "'></div>"
                + "<div><label>Phone Number: </label><label hidden id='usereditnumhid'>" + user.PhoneNumber + "</label><input type='text' id='usereditnum' maxlength='10' pattern='^[0-9]*$' value='" + user.PhoneNumber + "'></div>"
                + "<div class='admin_dashboard_content_users_popup_overlay_content_info_details_btns'>"
                + "<button class='admin_dashboard_content_users_popup_overlay_content_info_details_btns_license' onclick='showLicense()' id='userlicensebtn'>View License</button>" 
                + "<button class='admin_dashboard_content_users_popup_overlay_content_info_details_btns_additional' onclick='showAdditional()' id='useradditionalbtn'>View Additional Identification</button>" 
                + "</div></div>";

            //html += "<div class='admin_dashboard_content_users_popup_overlay_content_info_btns'>"
            //    + "<button class='admin_dashboard_content_users_popup_overlay_content_info_btns_edit' onclick='enableUserInput()' id='usereditbtn'>Edit</button>"
            //    + "<button class='admin_dashboard_content_users_popup_overlay_content_info_btns_license' id='usereditlicensebtn'>View License</button></div>"
            //    + "<div class='admin_dashboard_content_users_popup_overlay_content_info_editbtns'>"
            //    + "<button class='admin_dashboard_content_users_popup_overlay_content_info_editbtns_save' id='usereditsavebtn'>Save</button>"
            //    + "<button class='admin_dashboard_content_users_popup_overlay_content_info_editbtns_cancel' onclick='cancelUserEdit()' id='usereditcancelbtn'>Cancel</button></div></div>";

            //html += "<div class='admin_dashboard_content_users_popup_overlay_content_info_btns'>"
            //    + "";

            //html += "<div class='admin_dashboard_content_users_popup_overlay_content_info_identity'>";

            if (user.License != null)
            {
                html += "<div class='admin_dashboard_content_users_popup_overlay_content_info_identity'>";

                html += "<img id='licenseimg' src='/User Document Images/" + user.License + "' hidden>"
                    + "<img id='additionalimg' src='/User Document Images/" + user.AdditonalIdentification + "' hidden>";
            }
            else
            {
                html += "<div class='admin_dashboard_content_users_popup_overlay_content_info_identity na'>";

                html += "<label id='licenseimg' hidden>N/A</label>"
                    + "<label id='additionalimg' hidden>N/A</label>";
            }

            html += "</div></div>";

            html += "<div class='admin_dashboard_content_users_popup_overlay_content_bookings'>"
                + "</div></div></div>";

            return Json(html);
        }

        [System.Web.Http.HttpPost]
        public JsonResult GetUpdatedUserDetails([FromUri] int id, int index)
        {
            String html = null;

            String currentRole = _userManager.GetRolesAsync(_userManager.FindByNameAsync(User.Identity.Name).Result).Result[0];

            User user = _userManager.FindByIdAsync(id.ToString()).Result;

            html = "<div class='admin_dashboard_content_users_users_user_status'>";

            if (user.Status.Equals("Unverified"))
            {
                html += "<span class='admin_dashboard_content_users_users_user_status_notverified'>NOT VERIFIED</span>";
            }
            else
            {
                if (user.Status.Equals("Blacklisted"))
                {
                    if (_bookingService.GetCountOfUser(user.Id) != 0)
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_blacklisted'>BLACKLISTED<svg onclick='removeBlacklist(" + user.Id + "," + index + ")' id='userremoveblacklist' xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z' /></svg></span>"
                             + "<span class='admin_dashboard_content_users_users_user_status_repeat'>REPEAT</span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                    else
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_blacklisted'>BLACKLISTED<svg onclick='removeBlacklist(" + user.Id + "," + index + ")' id='userremoveblacklist' xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M24 20.188l-8.315-8.209 8.2-8.282-3.697-3.697-8.212 8.318-8.31-8.203-3.666 3.666 8.321 8.24-8.206 8.313 3.666 3.666 8.237-8.318 8.285 8.203z' /></svg></span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                }
                else
                {
                    if (_bookingService.GetCountOfUser(user.Id) != 0)
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_repeat'>REPEAT</span>"
                            + "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                    else
                    {
                        html += "<span class='admin_dashboard_content_users_users_user_status_verified'>VERIFIED</span>";
                    }
                }
            }

            html += "</div><div class='admin_dashboard_content_users_users_user_role'>"
                + "<label>" + user.Role + "</label>";

            if (currentRole.Equals("SuperUser"))
            {
                if (user.Role.Equals("Client"))
                {
                    html += "<button id='usermakeadmin' onclick='toggleAdminPriviledge(" + user.Id + "," + index + ")'>Grant Admin Privileges</button>";
                }
                else
                {
                    html += "<button id='usermakeclient' onclick='toggleAdminPriviledge(" + user.Id + "," + index + ")'>Revoke Admin Privileges</button>";
                }
            }

            html += "</div>"
                    + "<label>USER #" + user.Id + "</label>"
                    + "<label>Name: <span>" + user.FullName + "</span></label>"
                    + "<label>E-mail Address: <span>" + user.Email + "</span></label>"
                    + "<label>Contact Number: <span>" + user.PhoneNumber + "</span></label>"
                    + "<div class='admin_dashboard_content_users_users_user_buttons'>"
                    + "<div>"
                    //+ "<button id='usermoreinfo' onclick='userEdit(" + user.Id + ", 0," + index + ")'>More Info</button><button onclick='userEdit(" + user.Id + ", 1)' id='useredit'>Edit</button></div><div>"
                    + "<button id='usermoreinfo' onclick='userEdit(" + user.Id + ", 0," + index + ")'>More Info</button></div><div>"
                    + "<button id='userremove' onclick='removeUser(" + user.Id + ")'>Remove User<svg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24'><path d='M9 19c0 .552-.448 1-1 1s-1-.448-1-1v-10c0-.552.448-1 1-1s1 .448 1 1v10zm4 0c0 .552-.448 1-1 1s-1-.448-1-1v-10c0-.552.448-1 1-1s1 .448 1 1v10zm4 0c0 .552-.448 1-1 1s-1-.448-1-1v-10c0-.552.448-1 1-1s1 .448 1 1v10zm5-17v2h-20v-2h5.711c.9 0 1.631-1.099 1.631-2h5.315c0 .901.73 2 1.631 2h5.712zm-3 4v16h-14v-16h-2v18h18v-18h-2z'/></svg></button>"
            + "</div></div>";

            return Json(html);
        }

        [System.Web.Http.HttpPost]
        public JsonResult RemoveBlacklist([FromUri] int id)
        {
            string result = _userService.RemoveBlacklist(id);

            return Json(result);
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> ToggleAdminPriviledgeAsync([FromUri] int id)
        {
            string result = await _userService.ToggleAdminPriviledgeAsync(id);

            return Json(result);
        }

        [System.Web.Http.HttpPost]
        public JsonResult RemoveUser([FromUri] int id)
        {
            //int numOfActiveBookings = _bookingService.GetCountOfUserActive(id);

            //if(numOfActiveBookings == 0)
            //{
            //    string result = _userService.RemoveUser(id);
            //    return Json(result);
            //}

            return Json("Active");
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> AddAdmin([FromUri] string name, string email, string phone, string pwd)
        {
            string username = Regex.Replace(name, @"\s+", "");

            User user = new User
            {
                FullName = name,
                Role = "Admin",
                UserName = username,
                Email = email,
                PhoneNumber = phone,
                Status = "Verified"
            };

            var result = await _userManager.CreateAsync(user, pwd);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Json("Success");
            }

            return Json("Failure");
        }


        //VEHICLE FUNCTIONS
        [System.Web.Http.HttpPost]
        public JsonResult GetVehicles([FromUri] int skip, string keyword, string type, string status)
        {
            List<string> html = new List<string>();
            List<Vehicle> vehciles = new List<Vehicle>();
            int count = 0;

            if (keyword != null)
            {
                if (!keyword.Equals(""))
                {
                    if (_vehicleService.SearchModelGetCount(keyword) != 0)
                    {
                        count = _vehicleService.SearchModelGetCount(keyword);
                        vehciles = _vehicleService.SearchModelGet10(keyword, skip);
                    }
                    else if (_vehicleService.SearchManufacturerGetCount(keyword) != 0)
                    {
                        count = _vehicleService.SearchManufacturerGetCount(keyword);
                        vehciles = _vehicleService.SearchManufacturerGet10(keyword, skip);
                    }
                    else if (_vehicleService.SearchCategoryGetCount(keyword) != 0)
                    {
                        count = _vehicleService.SearchCategoryGetCount(keyword);
                        vehciles = _vehicleService.SearchCategoryGet10(keyword, skip);
                    }
                    else if (_vehicleService.SearchFuelGetCount(keyword) != 0)
                    {
                        count = _vehicleService.SearchFuelGetCount(keyword);
                        vehciles = _vehicleService.SearchFuelGet10(keyword, skip);
                    }
                    else if (_vehicleService.SearchYearGetCount(Int32.Parse(keyword)) != 0)
                    {
                        count = _vehicleService.SearchYearGetCount(Int32.Parse(keyword));
                        vehciles = _vehicleService.SearchYearGet10(Int32.Parse(keyword), skip);
                    }
                }
                else
                {
                    count = _vehicleService.GetCount();
                    if (skip == 0)
                    {
                        vehciles = _vehicleService.Get10(0);
                    }
                    else
                    {
                        vehciles = _vehicleService.Get10(skip);
                    }
                }
            }
            //else if (type != null && status != null)
            //{
            //    count = _vehicleService.SearchStatusTypeGetCount(status,type);
            //    if (skip == 0)
            //    {
            //        vehciles = _vehicleService.SearchStatusTypeGet10(status, type, 0);
            //    }
            //    else
            //    {
            //        vehciles = _vehicleService.SearchStatusTypeGet10(status, type, skip);
            //    }
            //}
            //else if (status != null)
            //{
            //    count = _vehicleService.SearchStatusGetCount(status);
            //    if (skip == 0)
            //    {
            //        vehciles = _vehicleService.SearchStatusGet10(status, 0);
            //    }
            //    else
            //    {
            //        vehciles = _vehicleService.SearchStatusGet10(status, skip);
            //    }
            //}
            else if (type != null)
            {
                count = _vehicleService.SearchTypeGetCount(type);
                if (skip == 0)
                {
                    vehciles = _vehicleService.SearchTypeGet10(type, 0);
                }
                else
                {
                    vehciles = _vehicleService.SearchTypeGet10(type, skip);
                }
            }
            else
            {
                count = _vehicleService.GetCount();
                if (skip == 0)
                {
                    vehciles = _vehicleService.Get10(0);
                }
                else
                {
                    vehciles = _vehicleService.Get10(skip);
                }
            }

            if (vehciles.Count != 0)
            {
                if (skip == 0)
                {
                    html.Add(count.ToString());
                }

                foreach (Vehicle vehicle in vehciles)
                {
                    int index = vehciles.IndexOf(vehicle);

                    string code = "<tr>"
                        + "<td>" + vehicle.Id + "</td>"
                        + "<td>" + vehicle.Manufacturer.ManufacturerName + "</td>" +
                        "<td>" + vehicle.Model + "</td>" +
                        "<td>" + vehicle.Year + "</td>" +
                        "<td>" + vehicle.Category +"</td>" +
                        "<td><button onclick='vehicleEdit(" + vehicle.Id + ", 0, " + index + ")'>More Details</button><button onclick='vehicleEdit(" + vehicle.Id + ", 1, " + index + ")'>Edit</button></td>" +
                        "</tr>";

                    html.Add(code);
                }
            }

            return Json(html);
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> AddVehicle(string manufacturer, string model, int year, string color, string category, string fuelType, string plate, string rate, IFormFile imageFile)
        {
            string location = "/Vehicle Images";
            string dbName = null;

            dbName = _fileUploader.UploadFileAsync(imageFile, model, location).Result;

            if (dbName != null)
            {
                Manufacturer man = _manufacturerService.GetFromName(manufacturer);

                if (man == null)
                {
                    _manufacturerService.AddManufacturer(new Manufacturer
                    {
                        ManufacturerName = manufacturer
                    });

                    man = _manufacturerService.GetFromName(manufacturer);
                }

                Vehicle vehicle = new Vehicle
                {
                    Manufacturer = man,
                    Model = model,
                    Year = year,
                    Color = color,
                    Category = category,
                    FuelType = fuelType,
                    PlateNumber = plate,
                    Rate = Double.Parse(rate),
                    ImageUrl = dbName
                };

                _vehicleService.AddVehicle(vehicle);

                return Json("Success");
            }

            return Json("Failure");
        }

        [System.Web.Http.HttpPost]
        public async Task<JsonResult> CheckPlate([FromUri] string plate)
        {
            if (_vehicleService.CheckPlate(plate) == null)
            {
                return Json("Success");
            }

            return Json("Failure");
        }


        //EQUIPMENT FUNCTIONS
        [System.Web.Http.HttpPost]
        public async Task<JsonResult> AddEquipment(string name, string stock, IFormFile imageFile)
        {
            string location = "/Equipment Images";
            string dbName = null;

            dbName = _fileUploader.UploadFileAsync(imageFile, name, location).Result;

            if (dbName != null)
            {
                Equipment equipment = new Equipment
                {
                    EquipmentName = name,
                    NumberOfPieces = Int32.Parse(stock),
                    ImageUrl = dbName
                };

                _equipmentService.AddEquipment(equipment);

                return Json("Success");
            }

            return Json("Failure");
        }
    }
}
