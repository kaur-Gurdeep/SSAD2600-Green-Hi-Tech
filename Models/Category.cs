using System;
using System.Collections.Generic;

namespace GreenHiTech.Models;

public partial class Category
{
    public int PkId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
