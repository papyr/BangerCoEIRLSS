using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public int NumberOfPieces { get; set; }
        public string ImageUrl { get; set; }
    }
}
