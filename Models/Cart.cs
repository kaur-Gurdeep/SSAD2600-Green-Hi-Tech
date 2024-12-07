using System;
using System.Collections.Generic;

namespace GreenHiTech.Models;

public partial class Cart
{
    public int PkId { get; set; }

    public int FkUserId { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    public virtual User FkUser { get; set; } = null!;
}
