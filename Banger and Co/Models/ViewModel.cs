using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banger_and_Co.Models
{
    public class ViewModel
    {
        public string ErrorMessage { get; set; }
        public bool Admin { get; set; }
        public HttpContext HttpContext { get; set; }
        public UserManager<User> UserManager { get; set; }
        public BookingService BookingService { get; set; }
        public DataContext DataContext { get; set; }
        public List<User> Users { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Equipment> Equipments { get; set; }
        public int NumberOfUsers { get; internal set; }
        public int NumberOfBookings { get; internal set; }
        public int NumberOfVehicles { get; internal set; }
        public int NumberOfEquipment { get; internal set; }
    }
}
