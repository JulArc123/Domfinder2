namespace DomFinder2.Models
{
    public class ChatViewModel
    {
        public string CurrentUserId { get; set; }
        public string OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }
}
