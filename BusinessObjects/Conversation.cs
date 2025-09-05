using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Conversation
{
    public int ConversationId { get; set; }

    public int UserId { get; set; }

    public string? Title { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User User { get; set; } = null!;
}
