using System;
using System.Collections.Generic;

namespace AccountService.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int CustomerId { get; set; }

    public decimal Balance { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
