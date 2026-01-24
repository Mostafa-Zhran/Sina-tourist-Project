using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Model
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;

        public decimal? AverageRate { get; set; }

        public bool IsActive { get; set; } = false;

        // FK → ApplicationUser (RestaurantAdmin)
        public string OwnerId { get; set; } = null!;
        public ApplicationUser Owner { get; set; } = null!;

        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

}
