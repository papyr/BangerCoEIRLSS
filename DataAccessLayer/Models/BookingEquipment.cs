using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class BookingEquipment
    {
        public int Id { get; set; }
        public Booking Booking { get; set; }
        public Equipment Equipment { get; set; }
    }
}
