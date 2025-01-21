using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.Models;

public partial class AddressDetail
{
    public int PkId { get; set; }

    public string Unit { get; set; } = null!;

    public string HouseNumber { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
