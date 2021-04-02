using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public string DatePlaced { get; set; }
        public string PickupDate { get; set; }
        public string ReturnDate { get; set; }
        public string Status { get; set; }
    }
}
