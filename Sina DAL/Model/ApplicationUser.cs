using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Sina_DAL.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

}
