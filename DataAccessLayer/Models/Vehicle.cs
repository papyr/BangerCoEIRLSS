using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Vehicle
    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        public int Id { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string Category { get; set; }
        public string FuelType { get; set; }
        public string PlateNumber { get; set; }
        public double Rate { get; set; }
        public string ImageUrl { get; set; }
    }
}
