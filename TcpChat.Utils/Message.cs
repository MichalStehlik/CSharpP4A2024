namespace TcpChat.Utils
{
    public record Message
    {
        public string Content { get; init; } = String.Empty;
        public string? Target { get; init; } // null if broadcast, username if private message
        public bool IsSystem { get; init; } = false; // true if system message, false if user message
    }
}
