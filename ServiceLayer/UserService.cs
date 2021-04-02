using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class UserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int GetCount()
        {
            return _context.Users.Count();
        }

        public int GetAdminUserCount()
        {
            return _context.Users.Where(u => !u.Role.Equals("SuperUser")).Count();
        }

        public int GetCountOfStatus(string status)
        {
            return _context.Users.Where(p => p.Status.Equals(status)).Count();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public List<User> Get10(int skip)
        {
            return _context.Users.Where(u => !u.Role.Equals("SuperUser")).Skip(skip).Take(10).ToList();
        }

        public List<User> Get4(int skip)
        {
            return _context.Users.Where(u => !u.Role.Equals("SuperUser")).Skip(skip).Take(4).ToList();
        }

        public List<User> SearchRoleStatusGet10(string role, string status, int skip)
        {
            if (status.Equals("Verified"))
            {
                return _context.Users.Where(p => (p.Status.Equals(status) || p.Status.Equals("Repeat") || p.Status.Equals("Blacklisted")) && p.Role.Equals(role)).Skip(skip).Take(10).ToList();
            }
            else
            {
                return _context.Users.Where(p => p.Status.Equals(status) && p.Role.Equals(role)).Skip(skip).Take(10).ToList();
            }
        }

        public List<User> SearchRoleGet10(string role, int skip)
        {
            return _context.Users.Where(p => p.Role.Equals(role)).Skip(skip).Take(10).ToList();
        }

        public List<User> SearchStatusGet10(string status, int skip)
        {
            if(status.Equals("Verified"))
            {
                return _context.Users.Where(p => (p.Status.Equals(status) || p.Status.Equals("Repeat") || p.Status.Equals("Blacklisted")) && !p.Role.Equals("SuperUser")).Skip(skip).Take(10).ToList();
            }
            else
            {
                return _context.Users.Where(p => p.Status.Equals(status) && !p.Role.Equals("SuperUser")).Skip(skip).Take(10).ToList();
            }
        }

        public List<User> SearchNameGet10(string keyword, int skip)
        {
            return _context.Users.Where(p => p.FullName.Contains(keyword)).Skip(skip).Take(10).ToList();
        }

        public List<User> SearchIdGet10(string keyword, int skip)
        {
            return _context.Users.Where(p => p.Id.ToString().Contains(keyword)).Skip(skip).Take(10).ToList();
        }

        public List<User> SearchEmailGet10(string keyword, int skip)
        {
            return _context.Users.Where(p => p.Email.Contains(keyword)).Skip(skip).Take(10).ToList();
        }

        public int SearchRoleStatusGetCount(string role, string status)
        {
            if (status.Equals("Verified"))
            {
                return _context.Users.Where(p => (p.Status.Equals(status) || p.Status.Equals("Repeat") || p.Status.Equals("Blacklisted")) && p.Role.Equals(role)).Count();
            }
            else
            {
                return _context.Users.Where(p => p.Status.Equals(status) && p.Role.Equals(role)).Count();
            }
        }

        public int SearchRoleGetCount(string role)
        {
            return _context.Users.Where(p => p.Role.Equals(role)).Count();
        }

        public int SearchStatusGetCount(string status)
        {
            if(status.Equals("Verified"))
            {
                return _context.Users.Where(p => (p.Status.Equals(status) || p.Status.Equals("Repeat") || p.Status.Equals("Blacklisted")) && !p.Role.Equals("SuperUser")).Count();
            }
            else
            {
                return _context.Users.Where(p => p.Status.Equals(status) && !p.Role.Equals("SuperUser")).Count();
            }
        }

        public int SearchNameGetCount(string keyword)
        {
            return _context.Users.Where(p => p.FullName.Contains(keyword)).Count();
        }

        public int SearchIdGetCount(string keyword)
        {
            return _context.Users.Where(p => p.Id.ToString().Contains(keyword)).Count();
        }

        public int SearchEmailGetCount(string keyword)
        {
            return _context.Users.Where(p => p.Email.Contains(keyword)).Count();
        }

        public string RemoveBlacklist(int id)
        {
            User user = _context.Users.Where(p => p.Id == id).FirstOrDefault();

            user.Status = "Verified";
            var result = _userManager.UpdateAsync(user).Result;

            if(result.Succeeded)
            {
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }

        public async Task<string> ToggleAdminPriviledgeAsync(int id)
        {
            User user = _context.Users.Where(p => p.Id == id).FirstOrDefault();
            var oldRole = await _userManager.GetRolesAsync(user);
            string newRole = null;

            if (user.Role.Equals("Admin"))
            {
                user.Role = "Client";
                newRole = "Client";
            }
            else if(user.Role.Equals("Client"))
            {
                user.Role = "Admin";
                newRole = "Admin";
            }

            var result = _userManager.UpdateAsync(user).Result;
            await _userManager.RemoveFromRoleAsync(user, oldRole[0]);
            await _userManager.AddToRoleAsync(user, newRole);

            if (result.Succeeded)
            {
                return "Success" + user.Role;
            }
            else
            {
                return "Failure";
            }
        }

        public string RemoveUser(int id)
        {
            User user = _context.Users.Where(p => p.Id == id).FirstOrDefault();

            var result = _userManager.DeleteAsync(user).Result;

            if(result.Succeeded)
            {
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }
    }
}
