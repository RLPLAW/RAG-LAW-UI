using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Setting
{
    public int SettingId { get; set; }

    public int UserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
