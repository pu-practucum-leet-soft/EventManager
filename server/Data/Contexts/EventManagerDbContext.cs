
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data.Contexts
{

    public class EventManagerDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<EventParticipant> EventParticipants { get; set; }

        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {
        }

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
        }

    }
}
