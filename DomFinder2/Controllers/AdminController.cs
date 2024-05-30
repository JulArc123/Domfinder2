using DomFinder2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DomFinder2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Wyświetl wszystkich użytkowników
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Wyświetl szczegóły użytkownika i umożliw zarządzanie nim
        public async Task<IActionResult> ManageUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new ManageUserViewModel
            {
                User = user,
                IsAdmin = roles.Contains("Admin"),
                IsBanned = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.Now
            };

            return View(model);
        }

        // Dodaj lub odbierz rolę administratora
        [HttpPost]
        public async Task<IActionResult> ToggleAdminRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction(nameof(ManageUser), new { id });
        }

        // Zablokuj użytkownika
        [HttpPost]
        public async Task<IActionResult> BanUser(string id, int days)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddDays(days));
            return RedirectToAction(nameof(ManageUser), new { id });
        }

        // Odbanuj użytkownika
        [HttpPost]
        public async Task<IActionResult> UnbanUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            return RedirectToAction(nameof(ManageUser), new { id });
        }
    }

    public class ManageUserViewModel
    {
        public IdentityUser User { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }
    }
}
