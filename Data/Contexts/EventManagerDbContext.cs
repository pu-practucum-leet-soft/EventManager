
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
                .HasKey(ep => new { ep.EventId, ep.UserId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.User)
                .WithMany()
                .HasForeignKey(ep => ep.UserId);
        }
    }
}
