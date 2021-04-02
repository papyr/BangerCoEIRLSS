using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string License { get; set; }
        public string AdditonalIdentification { get; set; }
        public string Status { get; set; }
        //public string Status { get; set; }
    }
}
