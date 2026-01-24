using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Model
{
    public class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; } = "Main Menu";

        // FK
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }

}
