
using EventManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data.Contexts
{
    /// <summary>
    /// 
    /// </summary>
    public class EventManagerDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Event> Events { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<EventParticipant> EventParticipants { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public EventManagerDbContext(DbContextOptions<EventManagerDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
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
