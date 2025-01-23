using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GreenHiTech.Models;

namespace GreenHiTech.ViewModels;

public partial class OrderVM
{
    [Display(Name = "Order Id")]
    public int PkId { get; set; }

    [Display(Name = "User Id")]
    public int FkUserId { get; set; }

    [Display(Name = "Order Date")]
    public DateOnly OrderDate { get; set; }

    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Payment Status")]
    public string Status { get; set; } = null!;

    
}

