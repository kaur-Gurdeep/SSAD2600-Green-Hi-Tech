using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GreenHiTech.ViewModels
{
    public class CartProductVM
    {
        public int PkId { get; set; }

        [Display(Name = "Product ID")]
        public int FkProductId { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
    }
}
