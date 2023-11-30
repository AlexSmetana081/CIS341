using Lab09Smetana.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lab09Smetana.Data
{
    public class WorkoutContext : DbContext
    {

        public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;
        public DbSet<Message> Messages { get; set; } = default!;
        public DbSet<TrackedWorkout> TrackedWorkouts { get; set; } = default!;
        public DbSet<Workout> Workouts { get; set; } = default!;
        public DbSet<WorkoutSet> WorkoutSets { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fix multiple cascade errors -- this will make Update-Database to run
            // but you'll want to check all of the OnDelete behaviors.
            modelBuilder
                .Entity<TrackedWorkout>()
                .HasOne(e => e.Account)
                .WithMany(e => e.Workouts)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Message>()
                .HasOne(e => e.Sender)
                .WithMany(e => e.Messages)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<WorkoutSet>()
                .HasOne(e => e.Workout)
                .WithMany(e => e.Exercises)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }

}

