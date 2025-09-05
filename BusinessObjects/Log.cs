using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Log
{
    public int LogId { get; set; }

    public string? LogMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }
}
