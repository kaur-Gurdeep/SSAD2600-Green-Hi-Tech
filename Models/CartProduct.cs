using System;
using System.Collections.Generic;

namespace GreenHiTech.Models;

public partial class CartProduct
{
    public int PkId { get; set; }

    public int FkCartId { get; set; }

    public int FkProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Cart FkCart { get; set; } = null!;

    public virtual Product FkProduct { get; set; } = null!;
}
