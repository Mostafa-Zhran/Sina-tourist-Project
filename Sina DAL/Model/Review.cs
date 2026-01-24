using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sina_DAL.Model
{
    public class Review
    {
        public int Id { get; set; }

        public byte Rate { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK (string)
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;
    }
}
