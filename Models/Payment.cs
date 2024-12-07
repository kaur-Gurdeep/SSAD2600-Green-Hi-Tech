using System;
using System.Collections.Generic;

namespace GreenHiTech.Models;

public partial class Payment
{
    public int PkId { get; set; }

    public int FkOderId { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public int TransactionId { get; set; }

    public virtual Order FkOder { get; set; } = null!;
}
