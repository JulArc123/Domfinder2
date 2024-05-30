using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DomFinder2.Models;

namespace DomFinder2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatMessage>().HasKey(m => m.Id);

            // Dodaj unikalny indeks dla ChatRoomId i Timestamp dla szybszego zapytania
            builder.Entity<ChatMessage>()
                .HasIndex(m => new { m.ChatRoomId, m.Timestamp });
        }
    }
}
