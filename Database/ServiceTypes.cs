using System;
using System.Collections.Generic;

namespace Database;

public partial class ServiceTypes
{
    public int Id { get; set; }

    public string Tag { get; set; } = null!;

    public int AverageServiceTimeMinutes { get; set; }
}
