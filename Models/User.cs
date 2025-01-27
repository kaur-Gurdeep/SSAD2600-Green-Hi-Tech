using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.Models;

public partial class User
{
    [Key]
    public int PkId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int? FkAddressId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual AddressDetail FkAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}