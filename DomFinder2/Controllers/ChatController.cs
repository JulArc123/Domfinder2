using DomFinder2.Data;
using DomFinder2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DomFinder2.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChatController> _logger;

        public ChatController(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<ChatController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Chat(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            _logger.LogInformation("Current user: {CurrentUser}", currentUser?.Id);
            var otherUser = await _userManager.FindByIdAsync(userId);
            _logger.LogInformation("Other user: {OtherUser}", otherUser?.Id);

            if (otherUser == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
                return NotFound();
            }

            var chatRoomId = GenerateChatRoomId(currentUser.Id, otherUser.Id);
            _logger.LogInformation("Chat room ID: {ChatRoomId}", chatRoomId);

            var messages = await _context.ChatMessages
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var model = new ChatViewModel
            {
                CurrentUserId = currentUser.Id,
                OtherUserId = otherUser.Id,
                OtherUserName = otherUser.UserName,
                Messages = messages
            };

            return View(model);
        }

        public async Task<IActionResult> Messages()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            _logger.LogInformation("Fetching chat rooms for user {UserId}", userId);

            var chatRooms = await _context.ChatMessages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .Select(m => new { m.ChatRoomId, OtherUserId = m.SenderId == userId ? m.ReceiverId : m.SenderId })
                .Distinct()
                .ToListAsync();

            if (!chatRooms.Any())
            {
                _logger.LogWarning("No chat rooms found for user {UserId}", userId);
            }

            var model = new List<ChatRoomViewModel>();

            foreach (var chatRoom in chatRooms)
            {
                var user = await _userManager.FindByIdAsync(chatRoom.OtherUserId);
                if (user != null && user.Id != userId) // Dodatkowy warunek, aby wykluczyć samego siebie
                {
                    model.Add(new ChatRoomViewModel
                    {
                        ChatRoomId = chatRoom.ChatRoomId,
                        UserId = user.Id,
                        UserEmail = user.Email
                    });
                }
                else if (user == null)
                {
                    _logger.LogWarning("User with ID {OtherUserId} not found", chatRoom.OtherUserId);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string receiverId, string message)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var chatRoomId = GenerateChatRoomId(currentUser.Id, receiverId);

            var chatMessage = new ChatMessage
            {
                SenderId = currentUser.Id,
                ReceiverId = receiverId,
                Message = message,
                Timestamp = DateTime.UtcNow,
                ChatRoomId = chatRoomId
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { userId = receiverId });
        }

        private string GenerateChatRoomId(string userId1, string userId2)
        {
            var ids = new[] { userId1, userId2 }.OrderBy(id => id).ToArray();
            return $"{ids[0]}_{ids[1]}";
        }
    }
}
