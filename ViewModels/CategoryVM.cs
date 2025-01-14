using GreenHiTech.Models;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class CategoryVM
    {
        [Required]
        [StringLength(75)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
