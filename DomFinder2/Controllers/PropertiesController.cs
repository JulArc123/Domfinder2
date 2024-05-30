using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DomFinder2.Data;
using DomFinder2.Models;
using DomFinder2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace DomFinder2.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment hostEnvironment, ILogger<PropertiesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: Properties
        public async Task<IActionResult> Index(string city, string district, decimal? minPrice, decimal? maxPrice, decimal? minArea, decimal? maxArea, int? minRooms, int? maxRooms, string transactionType)
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(p => p.City.Contains(city));
            }

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(p => p.District.Contains(district));
            }

            if (!string.IsNullOrEmpty(transactionType))
            {
                if (transactionType.Equals("sprzedaż", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Price > 50000);
                }
                else if (transactionType.Equals("wynajem", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.Price <= 50000);
                }
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            if (minArea.HasValue)
            {
                query = query.Where(p => p.Area >= minArea);
            }

            if (maxArea.HasValue)
            {
                query = query.Where(p => p.Area <= maxArea);
            }

            if (minRooms.HasValue)
            {
                query = query.Where(p => p.Rooms >= minRooms);
            }

            if (maxRooms.HasValue)
            {
                query = query.Where(p => p.Rooms <= maxRooms);
            }

            ViewBag.City = city;
            ViewBag.District = district;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinArea = minArea;
            ViewBag.MaxArea = maxArea;
            ViewBag.MinRooms = minRooms;
            ViewBag.MaxRooms = maxRooms;
            ViewBag.TransactionType = transactionType;

            var properties = await query.ToListAsync();

            var propertyViewModels = properties.Select(p => new PropertyViewModel
            {
                Property = p,
                IsEditable = p.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin")
            }).ToList();

            return View(propertyViewModels);
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _context.Properties
                .FirstOrDefaultAsync(m => m.PropertyID == id);
            if (property == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(property.UserId);
            if (user != null)
            {
                property.UserName = user.UserName;
                property.UserPhoneNumber = user.PhoneNumber;
            }

            return View(property);
        }

        // GET: Properties/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Properties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("PropertyID,Title,Price,Area,Rooms,Description,City,District,Street,BuildingType,Floor,TotalFloors,YearBuilt,OwnershipType,AvailableFrom,Rent,AdvertiserType,EnergyCertificate,Internet,CableTV,Phone,AntiBurglaryBlinds,Monitoring,AlarmSystem,Intercom,ClosedArea,Furnished,WashingMachine,Fridge,Stove,Television,Dishwasher,Oven,ImagePaths,UserId")] Property property, List<IFormFile> Photos)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            property.UserId = user.Id;

            ModelState.Remove(nameof(Property.UserName));
            ModelState.Remove(nameof(Property.UserPhoneNumber));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return View(property);
            }

            if (Photos != null && Photos.Count > 0)
            {
                var photoPaths = new List<string>();
                foreach (var photo in Photos)
                {
                    if (photo.Length > 0)
                    {
                        var fileName = Path.GetFileName(photo.FileName);
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        photoPaths.Add("/images/" + fileName);
                    }
                }
                property.ImagePaths = photoPaths;
            }

            _context.Add(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit action called with null id");
                return NotFound();
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                _logger.LogWarning($"Property with id {id} not found");
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (property.UserId != user.Id && !User.IsInRole("Admin")))
            {
                _logger.LogWarning($"Unauthorized access attempt by user {user?.Id} for property {property.PropertyID}");
                return Unauthorized();
            }

            return View(property);
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("PropertyID,Title,Price,Area,Rooms,Description,City,District,Street,BuildingType,Floor,TotalFloors,YearBuilt,OwnershipType,AvailableFrom,Rent,AdvertiserType,EnergyCertificate,Internet,CableTV,Phone,AntiBurglaryBlinds,Monitoring,AlarmSystem,Intercom,ClosedArea,Furnished,WashingMachine,Fridge,Stove,Television,Dishwasher,Oven,UserId,ImagePaths,UserName,UserPhoneNumber")] Property property, List<IFormFile> Photos)
        {
            if (id != property.PropertyID)
            {
                _logger.LogWarning($"Mismatched PropertyID: {id} does not match {property.PropertyID}");
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (property.UserId != user.Id && !User.IsInRole("Admin")))
            {
                _logger.LogWarning($"Unauthorized access attempt by user {user?.Id} for property {property.PropertyID}");
                return Unauthorized();
            }

            ModelState.Remove(nameof(Property.UserName));
            ModelState.Remove(nameof(Property.UserPhoneNumber));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return View(property);
            }

            if (Photos != null && Photos.Count > 0)
            {
                var photoPaths = new List<string>();
                foreach (var photo in Photos)
                {
                    if (photo.Length > 0)
                    {
                        var fileName = Path.GetFileName(photo.FileName);
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        photoPaths.Add("/images/" + fileName);
                    }
                }
                property.ImagePaths = photoPaths;
            }
            else
            {
                _context.Entry(property).Property(p => p.ImagePaths).IsModified = false;
            }

            try
            {
                _context.Update(property);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PropertyExists(property.PropertyID))
                {
                    _logger.LogWarning($"Concurrency exception: Property {property.PropertyID} does not exist");
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Concurrency exception", ex);
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete action called with null id");
                return NotFound();
            }

            var property = await _context.Properties
                .FirstOrDefaultAsync(m => m.PropertyID == id);
            if (property == null)
            {
                _logger.LogWarning($"Property with id {id} not found");
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var viewModel = new PropertyViewModel
            {
                Property = property,
                IsEditable = property.UserId == userId || User.IsInRole("Admin")
            };

            return View(viewModel);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (property == null)
            {
                _logger.LogWarning($"DeleteConfirmed action: Property with id {id} not found");
                return NotFound();
            }

            if (user == null || (property.UserId != user.Id && !User.IsInRole("Admin")))
            {
                _logger.LogWarning($"Unauthorized delete attempt by user {user?.Id} for property {property.PropertyID}");
                return Unauthorized();
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.PropertyID == id);
        }
    }
}
