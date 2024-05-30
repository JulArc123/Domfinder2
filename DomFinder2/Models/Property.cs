using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DomFinder2.Models
{
    public class Property
    {
        public int PropertyID { get; set; }

        [Display(Name = "Tytuł ogłoszenia")]
        public string Title { get; set; }

        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Display(Name = "Powierzchnia")]
        public decimal Area { get; set; }

        [Display(Name = "Liczba pokoi")]
        public int Rooms { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Dzielnica")]
        public string District { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Rodzaj zabudowy")]
        public string BuildingType { get; set; }

        [Display(Name = "Piętro")]
        public int? Floor { get; set; }

        [Display(Name = "Liczba pięter")]
        public int? TotalFloors { get; set; }

        [Display(Name = "Rok budowy")]
        public int? YearBuilt { get; set; }

        [Display(Name = "Forma własności")]
        public string OwnershipType { get; set; }

        [Display(Name = "Dostępne od")]
        public DateTime? AvailableFrom { get; set; }

        [Display(Name = "Czynsz")]
        public decimal? Rent { get; set; }

        [Display(Name = "Typ ogłoszeniodawcy")]
        public string AdvertiserType { get; set; }

        [Display(Name = "Certyfikat energetyczny")]
        public string EnergyCertificate { get; set; }

        [Display(Name = "Internet")]
        public bool Internet { get; set; }

        [Display(Name = "Telewizja kablowa")]
        public bool CableTV { get; set; }

        [Display(Name = "Telefon")]
        public bool Phone { get; set; }

        [Display(Name = "Rolety antywłamaniowe")]
        public bool AntiBurglaryBlinds { get; set; }

        [Display(Name = "Monitoring")]
        public bool Monitoring { get; set; }

        [Display(Name = "System alarmowy")]
        public bool AlarmSystem { get; set; }

        [Display(Name = "Domofon / wideofon")]
        public bool Intercom { get; set; }

        [Display(Name = "Teren zamknięty")]
        public bool ClosedArea { get; set; }

        [Display(Name = "Umeblowane")]
        public bool Furnished { get; set; }

        [Display(Name = "Pralka")]
        public bool WashingMachine { get; set; }

        [Display(Name = "Lodówka")]
        public bool Fridge { get; set; }

        [Display(Name = "Kuchenka")]
        public bool Stove { get; set; }

        [Display(Name = "Telewizor")]
        public bool Television { get; set; }

        [Display(Name = "Zmywarka")]
        public bool Dishwasher { get; set; }

        [Display(Name = "Piekarnik")]
        public bool Oven { get; set; }

        public string? UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; } = null!;

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string UserPhoneNumber { get; set; }

        public List<string> ImagePaths { get; set; } = new List<string>();
    }
}
