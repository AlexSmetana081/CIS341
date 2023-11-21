using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WorkoutContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            if (context.Accounts.Any())
            {
                return;
            }

            var account = new Account()
            {
                //AccountId = 1,
                Name = "Name1@gmail.com",
            };

            var account2 = new Account()
            {
                //AccountId = 1,
                Name = "Name2@gmail.com",
            };

            var account3 = new Account()
            {
                //AccountId = 1,
                Name = "Name3@gmail.com",
            };

            // Add Accounts to DbSet
            context.Accounts.Add(account);
            context.Accounts.Add(account2);
            context.Accounts.Add(account3);
            context.SaveChanges();

            var exercise = new Exercise()
            {
                //ExerciseId = 1,
                Name = "Name1",
                Description = "Description1",
                WorkoutIntensity = Intensity.Low,
                AuthorName = context.Accounts.First(),

            };

            // Add Role to DbSet
            context.Exercises.Add(exercise);

            context.SaveChanges();


            var workout = new Workout()
            {
                Name = "name1",
                Description = "description1",
                Author = context.Accounts.First(),
                Exercises = new List<WorkoutSet>()
                {
                    new WorkoutSet()
                    {
                         Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 1),
                          // Replace 1 with the actual ExerciseId
                    }
                }
            };

            //Add Workout to DbSet
            context.Workouts.Add(workout);

            context.SaveChanges();

            var workoutSet = new WorkoutSet()
            {
                Exercise = context.Exercises.First(),
                Workout = context.Workouts.First(),

            };

            context.WorkoutSets.Add(workoutSet);
            context.SaveChanges();

            var message = new Message()
            {
                Content = "content1",
                Sender = context.Accounts.First(),
                Recipient = context.Accounts.Skip(1).First(),
                DateSent = DateTime.Now,
            };

            // Add Message to DbSet
            context.Messages.Add(message);

            context.SaveChanges();

            var TrackedWorkoutModel = new TrackedWorkout()
            {
                Account = context.Accounts.First(),
                Workout = context.Workouts.First(),
                Completed = DateTime.Now
            };

            ////Add Tracked Workout to Dbset
            context.TrackedWorkouts.Add(TrackedWorkoutModel);

            context.SaveChanges();
        }
    }
}
