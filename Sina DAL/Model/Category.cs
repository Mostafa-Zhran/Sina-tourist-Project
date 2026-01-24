using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Model
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        // FK
        public int MenuId { get; set; }
        public Menu Menu { get; set; } = null!;

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }

}
