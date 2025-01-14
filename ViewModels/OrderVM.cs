using System;
using System.Collections.Generic;
using GreenHiTech.Models;

namespace GreenHiTech.ViewModels;

public partial class OrderVM
{
    public int PkId { get; set; }

    public int FkUserId { get; set; }

    public DateOnly OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual User FkUser { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

