using GreenHiTech.Models;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class CartVM
    {
        [Display(Name = "Cart ID")]
        public int PkId { get; set; }

        [Display(Name = "User ID")]
        public int FkUserId { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Products")]
        public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    }
}
