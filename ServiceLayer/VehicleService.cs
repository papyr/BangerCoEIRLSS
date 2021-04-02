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
    public class VehicleService
    {
        private readonly DataContext _context;

        public VehicleService(DataContext context)
        {
            _context = context;
        }


        public void AddVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        public Vehicle CheckPlate(string plate)
        {
            return _context.Vehicles.Where(v => v.PlateNumber.Equals(plate.ToLower())).FirstOrDefault();
        }

        public int GetCount()
        {
            return _context.Vehicles.Count();
        }

        public List<Vehicle> Get10(int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Skip(skip).Take(10).ToList();
        }


        //SEARCH FUNCTIONS
        public int SearchModelGetCount(string keyword)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Model.ToLower().Contains(keyword.ToLower())).Count();
        }
        public List<Vehicle> SearchModelGet10(string keyword, int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Model.ToLower().Contains(keyword.ToLower())).Skip(skip).Take(10).ToList();
        }

        public int SearchManufacturerGetCount(string keyword)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Manufacturer.ManufacturerName.ToLower().Contains(keyword.ToLower())).Count();
        }
        public List<Vehicle> SearchManufacturerGet10(string keyword, int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Manufacturer.ManufacturerName.ToLower().Contains(keyword.ToLower())).Skip(skip).Take(10).ToList();
        }

        public int SearchCategoryGetCount(string keyword)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Category.ToLower().Contains(keyword.ToLower())).Count();
        }
        public List<Vehicle> SearchCategoryGet10(string keyword, int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Category.ToLower().Contains(keyword.ToLower())).Skip(skip).Take(10).ToList();
        }

        public int SearchFuelGetCount(string keyword)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.FuelType.ToLower().Contains(keyword.ToLower())).Count();
        }
        public List<Vehicle> SearchFuelGet10(string keyword, int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.FuelType.ToLower().Contains(keyword.ToLower())).Skip(skip).Take(10).ToList();
        }

        public int SearchYearGetCount(int keyword)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Year == keyword).Count();
        }
        public List<Vehicle> SearchYearGet10(int keyword, int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Year == keyword).Skip(skip).Take(10).ToList();
        }

        //public int SearchStatusTypeGetCount(string status, string type)
        //{
        //    return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Year == keyword).Count();
        //}
        //public List<Vehicle> SearchStatusTypeGet10(string status, string type, int skip)
        //{
        //    return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Year == keyword).Skip(skip).Take(10).ToList();
        //}

        //public int SearchStatusGetCount(string status)
        //{
        //    return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Year == keyword).Count();
        //}
        //public List<Vehicle> SearchStatusGet10(string status, int skip)
        //{
        //    return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Year == keyword).Skip(skip).Take(10).ToList();
        //}

        public int SearchTypeGetCount(string type)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Category.Equals(type)).Count();
        }
        public List<Vehicle> SearchTypeGet10(string type, int skip)
        {
            return _context.Vehicles.Include(ui => ui.Manufacturer).Where(p => p.Category.Equals(type)).Skip(skip).Take(10).ToList();
        }
    }
}
