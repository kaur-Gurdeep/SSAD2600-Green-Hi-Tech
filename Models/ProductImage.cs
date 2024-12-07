using System;
using System.Collections.Generic;

namespace GreenHiTech.Models;

public partial class ProductImage
{
    public int PkId { get; set; }

    public string AltText { get; set; } = null!;

    public int FkProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateOnly CreateDate { get; set; }

    public virtual Product FkProduct { get; set; } = null!;
}
