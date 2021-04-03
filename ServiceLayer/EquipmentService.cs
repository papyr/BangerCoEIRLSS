using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class EquipmentService
    {
        private readonly DataContext _context;

        public EquipmentService(DataContext context)
        {
            _context = context;
        }


        public void AddEquipment(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            _context.SaveChanges();
        }

        public int GetCount()
        {
            return _context.Equipment.Count();
        }

        public List<Equipment> Get10(int skip)
        {
            return _context.Equipment.Skip(skip).Take(10).ToList();
        }


        //SEARCH FUNCTIONS
        public int SearchNameGetCount(string name)
        {
            return _context.Equipment.Where(p => p.EquipmentName.ToLower().Contains(name.ToLower())).Count();
        }
        public List<Equipment> SearchNameGet10(string name, int skip)
        {
            return _context.Equipment.Where(p => p.EquipmentName.ToLower().Contains(name.ToLower())).Skip(skip).Take(10).ToList();
        }

        public int SearchIdGetCount(string id)
        {
            return _context.Equipment.Where(p => p.Id.ToString().Contains(id)).Count();
        }
        public List<Equipment> SearchIdGet10(string id, int skip)
        {
            return _context.Equipment.Where(p => p.Id.ToString().Contains(id)).Skip(skip).Take(10).ToList();
        }

    }
}
