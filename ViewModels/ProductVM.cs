using GreenHiTech.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GreenHiTech.ViewModels
{
    public class ProductVM
    {
        [Required]
        [StringLength(50)]

        public string Name { get; set; } = null!;

        [Required]
        [StringLength(250)]
        public string Description { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; } = null!;

        [Required]
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();


    }
}
