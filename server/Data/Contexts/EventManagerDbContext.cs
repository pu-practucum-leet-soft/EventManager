
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data.Contexts
{
    /// <summary>
    /// Главният DbContext за EventManager приложението.
    /// Управлява достъпа до базата данни чрез Entity Framework Core
    /// и включва интеграция с ASP.NET Core Identity за работа с потребители и роли.
    /// </summary>
    public class EventManagerDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Представлява таблицата с всички събития (Events).
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Представлява връзките между потребители и събития (участници).
        /// </summary>
        public DbSet<EventParticipant> EventParticipants { get; set; }

        /// <summary>
        /// Представлява таблицата с refresh токени, използвани за подновяване на JWT.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Създава нов екземпляр на <see cref="EventManagerDbContext"/> 
        /// с предоставените опции за базата данни.
        /// </summary>
        /// <param name="options">Опции за конфигурация на DbContext.</param>
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Конфигурира схемата на базата данни и връзките между ентитетите.
        /// Определя имена на таблици, ограничения, индекси и релации.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder обект за конфигурация на EF Core модела.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Дефолтна schema
            modelBuilder.HasDefaultSchema("app");

            // Events
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Description)
                      .HasMaxLength(2000);

                entity.Property(e => e.Location)
                      .HasMaxLength(50);
            });

            // EventParticipant
            modelBuilder.Entity<EventParticipant>(entity =>
            {
                entity.ToTable("EventParticipants");

                entity.HasIndex(ep => new { ep.EventId, ep.InviteeId })
                      .IsUnique();

                entity.HasOne(ep => ep.Event)
                      .WithMany(e => e.Participants)
                      .HasForeignKey(ep => ep.EventId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ep => ep.Invitee)
                      .WithMany()
                      .HasForeignKey(ep => ep.InviteeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ep => ep.Inviter)
                      .WithMany()
                      .HasForeignKey(ep => ep.InviterId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.ToTable("RefreshTokens");
                e.HasIndex(x => x.Token).IsUnique();
                e.Property(x => x.Token).HasMaxLength(512).IsRequired();
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
