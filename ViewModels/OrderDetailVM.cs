﻿using GreenHiTech.Models;
using System;
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

        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }

        [Display(Name = "Total")]
        public decimal Total => Quantity * Price;

        // Navigational properties
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Display(Name = "Product Image")]
        public ProductImage? ProductImage { get; set; }

    }
}
