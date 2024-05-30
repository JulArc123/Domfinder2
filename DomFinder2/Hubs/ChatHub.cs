using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DomFinder2.Models;
using DomFinder2.Data;
using Microsoft.EntityFrameworkCore;

namespace DomFinder2.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;
            var chatRoomId = GenerateChatRoomId(senderId, receiverId);

            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.UtcNow,
                ChatRoomId = chatRoomId
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var otherUserId = Context.GetHttpContext().Request.Query["userId"];
            var chatRoomId = GenerateChatRoomId(userId, otherUserId);

            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);

            await base.OnConnectedAsync();
        }

        private string GenerateChatRoomId(string userId1, string userId2)
        {
            return string.CompareOrdinal(userId1, userId2) < 0
                ? $"{userId1}_{userId2}"
                : $"{userId2}_{userId1}";
        }
    }
}
