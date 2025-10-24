using System;
using System.Collections.Generic;

namespace Database;

public partial class Tickets
{
    public int Id { get; set; }

    public string TicketCode { get; set; } = null!;

    public string ServiceType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsServed { get; set; }

    public DateTime? ServedAt { get; set; }

    public int? CounterId { get; set; }

    public DateTime? CalledAt { get; set; }
}
