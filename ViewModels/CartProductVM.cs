using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class CartProductVM
    {
        public int PkId { get; set; }

        [Display(Name = "Cart")]
        public int FkCartId { get; set; }

        [Display(Name = "Product")]
        public int FkProductId { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}
