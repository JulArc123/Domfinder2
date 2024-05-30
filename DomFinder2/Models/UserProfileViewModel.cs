using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DomFinder2.Models
{
    public class UserProfileViewModel
    {
        public IdentityUser User { get; set; }
        public List<Property> Properties { get; set; }
    }
}
