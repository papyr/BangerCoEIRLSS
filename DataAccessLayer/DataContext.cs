using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataContext : IdentityDbContext<User, UserRole, int>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingEquipment> BookingEquipment { get; set; }
    }
}
