using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Model
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        // FK
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }

}
