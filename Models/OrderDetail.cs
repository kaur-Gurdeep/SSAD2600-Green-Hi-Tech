using System;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.Models
{
    public class OrderDetail
    {
        public int PkId { get; set; }

        [Required]
        public int FkOrderId { get; set; }

        [Required]
        public int FkProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public virtual Order FkOrder { get; set; }
        public virtual Product FkProduct { get; set; }

    }
}
