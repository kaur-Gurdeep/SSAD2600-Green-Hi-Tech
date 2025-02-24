﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class OrderDetailVM
    {
        public int PkId { get; set; }

        [Required]
        [Display(Name = "Order ID")]
        public int FkOrderId { get; set; }

        [Required]
        [Display(Name = "Product ID")]
        public int FkProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Additional fields for display purposes
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }
    }
}
