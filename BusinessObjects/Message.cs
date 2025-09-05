using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Message
{
    public int MessageId { get; set; }

    public int ConversationId { get; set; }

    public string? UserMessage { get; set; }

    public string? ChatResponse { get; set; }

    public DateTime TimeStamp { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;
}
