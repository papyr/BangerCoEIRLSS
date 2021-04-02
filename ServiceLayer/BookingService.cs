using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class BookingService
    {
        private readonly DataContext _context;

        public BookingService(DataContext context)
        {
            _context = context;
        }


        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public int GetCount()
        {
            return _context.Bookings.Count();
        }

        public int GetCountOfStatus(string status)
        {
            return _context.Bookings.Where(p => p.Status.Equals(status)).Count();
        }

        public int GetCountOfUser(int id)
        {
            return _context.Bookings.Include(ui => ui.User).Where(p => p.User.Id == id).Count();
        }

        public int GetCountOfUserActive(int id)
        {
            return _context.Bookings.Include(ui => ui.User).Where(p => (p.User.Id == id) && (p.Status.Equals("Active"))).Count();
        }

        public int GetCountOfVehicle(int id)
        {
            return _context.Bookings.Include(ui => ui.Vehicle).Where(p => p.Vehicle.Id == id).Count();
        }

        public List<Booking> Get10(int skip)
        {
            return _context.Bookings.Include(ui => ui.User).Include(ui => ui.Vehicle).Skip(skip).Take(10).ToList();
        }
    }
}
