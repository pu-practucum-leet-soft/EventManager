
using Microsoft.EntityFrameworkCore;
using EventManager.Data.Entities;

namespace EventManager.Data.Contexts
{

    public class EventManagerDbContext : DbContext 
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<EventParticipant> EventParticipants { get; set; } = default!;

        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventParticipant>()
    .HasKey(ep => new { ep.EventId, ep.InviteeId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade); // трий участниците при триене на Event

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Invitee)
                .WithMany()
                .HasForeignKey(ep => ep.InviteeId)
                .OnDelete(DeleteBehavior.Restrict); // НЕ каскадирай от User

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Inviter)
                .WithMany()
                .HasForeignKey(ep => ep.InviterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
