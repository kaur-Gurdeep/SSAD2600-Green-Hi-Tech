using System;
using System.Collections.Generic;

namespace GreenHiTech.Models;

public partial class OrderDetail
{
    public int PkId { get; set; }

    public int FkOrderId { get; set; }

    public int FkProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Order FkOrder { get; set; } = null!;

    public virtual Product FkProduct { get; set; } = null!;
}
