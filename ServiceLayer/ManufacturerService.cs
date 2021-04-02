using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ManufacturerService
    {
        private readonly DataContext _context;

        public ManufacturerService(DataContext context)
        {
            _context = context;
        }


        public void AddManufacturer(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();
        }

        public int GetCount()
        {
            return _context.Manufacturers.Count();
        }

        public List<Manufacturer> Get10(int skip)
        {
            return _context.Manufacturers.Skip(skip).Take(10).ToList();
        }

        public List<Manufacturer> GetAll()
        {
            return _context.Manufacturers.ToList();
        }

        public Manufacturer GetFromName(string manufacturerName)
        {
            return _context.Manufacturers.Where(m => m.ManufacturerName.Equals(manufacturerName)).FirstOrDefault();
        }
    }
}
