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
    }
}
