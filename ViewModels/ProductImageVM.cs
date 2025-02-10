using GreenHiTech.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenHiTech.ViewModels
{
    public class ProductImageVM
    {
        //public int PkId { get; set; }
        [Required]
        [StringLength(150)]
        public string AltText { get; set; } = null!;

        [Required]
        [Column(TypeName = "int")]
        public int FkProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Column(TypeName = "DateOnly")]
        public DateOnly CreateDate { get; set; }

        [Required]
        [StringLength(150)]
        public virtual Product FkProduct { get; set; } = null!;
    }
}
