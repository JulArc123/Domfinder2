using DomFinder2.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DomFinder2.ViewModels
{
    public class PropertyViewModel
    {
        public Property Property { get; set; }
        public bool IsEditable { get; set; }
    }
}
