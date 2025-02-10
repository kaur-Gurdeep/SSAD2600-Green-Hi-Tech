using GreenHiTech.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GreenHiTech.ViewModels
{
    public class ProductVM
    {
        [Column(TypeName = "int")]
        [Display(Name = "Product ID")]
        public int PkId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price (CAD)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "int")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; } = 0;

        [Required]
        [Column(TypeName = "int")]
        [Display(Name = "Category")]
        public int FkCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; } = null!;

        [Required]
        [Display(Name = "Product Images")]
        public virtual ICollection<ProductImageVM> ProductImageVMs { get; set; } = new List<ProductImageVM>();

        //public ICollection<Category> Categories { get; set; } = null!;


    }
}
