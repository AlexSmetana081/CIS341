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

            /*
             * Account Population 10 Pages
             */

            var account1 = new Account()
            {
                Name = "john.doe@gmail.com",
            };

            var account2 = new Account()
            {
                Name = "jane.smith@gmail.com",
            };

            var account3 = new Account()
            {
                Name = "bob.jones@gmail.com",
            };

            var account4 = new Account()
            {
                Name = "emma.white@gmail.com",
            };

            var account5 = new Account()
            {
                Name = "michael.jordan@gmail.com",
            };

            var account6 = new Account()
            {
                Name = "olivia.brown@gmail.com",
            };

            var account7 = new Account()
            {
                Name = "david.miller@gmail.com",
            };

            var account8 = new Account()
            {
                Name = "sophia.wilson@gmail.com",
            };

            var account9 = new Account()
            {
                Name = "chris.evans@gmail.com",
            };

            var account10 = new Account()
            {
                Name = "lisa.green@gmail.com",
            };

            // Add Accounts to DbSet
            context.Accounts.AddRange(account1, account2, account3, account4, account5, account6, account7, account8, account9, account10);
            context.SaveChanges();






            /*
            * Exercise Population 10 Pages
            */

            var exercise1 = new Exercise()
            {
                Name = "Jumping Jacks",
                Description = "A cardio exercise involving jumping while spreading the legs and arms wide and then returning to a position with the feet together and arms at the sides.",
                WorkoutIntensity = Intensity.Medium,
                AuthorName = account1,
            };

            var exercise2 = new Exercise()
            {
                Name = "Deadlifts",
                Description = "A compound weightlifting exercise where you lift a loaded barbell or dumbbells from the ground to a standing position.",
                WorkoutIntensity = Intensity.High,
                AuthorName = account2,
            };

            var exercise3 = new Exercise()
            {
                Name = "Mountain Climbers",
                Description = "A full-body exercise that involves starting in a plank position and bringing alternate knees to the chest in a running motion.",
                WorkoutIntensity = Intensity.Medium,
                AuthorName = account3,
            };

            var exercise4 = new Exercise()
            {
                Name = "Tricep Dips",
                Description = "An exercise that targets the triceps, involving lowering and raising the body using parallel bars or a sturdy surface.",
                WorkoutIntensity = Intensity.Low,
                AuthorName = account4,
            };

            var exercise5 = new Exercise()
            {
                Name = "Burpees",
                Description = "A full-body exercise that combines a squat, push-up, and jump, providing both strength and aerobic benefits.",
                WorkoutIntensity = Intensity.High,
                AuthorName = account5,
            };

            var exercise6 = new Exercise()
            {
                Name = "Russian Twists",
                Description = "An abdominal exercise where you sit on the ground, lean back, and twist your torso to touch the ground on either side of you.",
                WorkoutIntensity = Intensity.Medium,
                AuthorName = account6,
            };

            var exercise7 = new Exercise()
            {
                Name = "Plank to Push-up",
                Description = "A dynamic exercise that involves moving from a plank position to a push-up position and back, engaging multiple muscle groups.",
                WorkoutIntensity = Intensity.High,
                AuthorName = account7,
            };

            var exercise8 = new Exercise()
            {
                Name = "Wall Sit",
                Description = "A lower-body exercise where you sit against a wall with your knees bent at a 90-degree angle, engaging the quadriceps and glutes.",
                WorkoutIntensity = Intensity.Medium,
                AuthorName = account8,
            };

            var exercise9 = new Exercise()
            {
                Name = "Box Jumps",
                Description = "An explosive exercise where you jump onto a sturdy box or platform, targeting the lower body and improving power.",
                WorkoutIntensity = Intensity.High,
                AuthorName = account9,
            };

            var exercise10 = new Exercise()
            {
                Name = "Side Plank",
                Description = "An isometric core exercise where you support your body sideways on one arm, keeping your body in a straight line.",
                WorkoutIntensity = Intensity.Low,
                AuthorName = account10,
            };

            // Add Exercises to DbSet
            context.Exercises.AddRange(exercise1, exercise2, exercise3, exercise4, exercise5, exercise6, exercise7, exercise8, exercise9, exercise10);
            context.SaveChanges();






            /*
            * Workout Population 10 Pages
            */

            var workout1 = new Workout()
            {
                Name = "Full Body Workout 1",
                Description = "A comprehensive workout targeting various muscle groups.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
                {
                    new WorkoutSet()
                    {
                         Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 1),
                          
                    }
                }
            };

            //Add Workout to DbSet
            context.Workouts.Add(workout1);

            var workout2 = new Workout()
            {
                Name = "Cardio Blast 2",
                Description = "A high-intensity cardio workout to boost your cardiovascular fitness.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
                {
                    new WorkoutSet()
                    {
                         Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 2),

                    }
                }
            };

            //Add Workout to DbSet
            context.Workouts.Add(workout2);

            // Workout 3
            var workout3 = new Workout()
            {
                Name = "Strength Training 3",
                Description = "Focus on building strength with this workout.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
                {
                    new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 5) },
                    new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 6) },
                }
            };
            context.Workouts.Add(workout3);

            // Workout 4
            var workout4 = new Workout()
            {
                Name = "Yoga Flow 4",
                Description = "A calming yoga flow to improve flexibility and relaxation.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
                {
                    new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 7) },
                    new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 8) },
                 }
            };
            context.Workouts.Add(workout4);

            // Workout 5
            var workout5 = new Workout()
            {
                Name = "High-Intensity Interval Training (HIIT) 5",
                Description = "An intense interval workout for maximum calorie burn.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
    {
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 9) },
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 10) },
    }
            };
            context.Workouts.Add(workout5);

            // Workout 6
            var workout6 = new Workout()
            {
                Name = "Pilates Core 6",
                Description = "Strengthen your core with this Pilates-focused workout.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
    {
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 1) },
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 2) },
                new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 6) },
    }
            };
            context.Workouts.Add(workout6);

            // Workout 7
            var workout7 = new Workout()
            {
                Name = "CrossFit Challenge 7",
                Description = "A challenging CrossFit workout for overall fitness.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
    {
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 3) },
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 4) },
                new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 1) },
    }
            };
            context.Workouts.Add(workout7);

            // Workout 8
            var workout8 = new Workout()
            {
                Name = "Cycling Adventure 8",
                Description = "Take your cardio outdoors with this cycling workout.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
    {
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 5) },
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 6) },
                new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 7) },
    }
            };
            context.Workouts.Add(workout8);

            // Workout 9
            var workout9 = new Workout()
            {
                Name = "Functional Fitness 9",
                Description = "Improve functional strength with this varied workout.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
    {
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 7) },
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 8) },
    }
            };
            context.Workouts.Add(workout9);

            // Workout 10
            var workout10 = new Workout()
            {
                Name = "Meditative Stretch 10",
                Description = "A gentle stretching routine for relaxation and flexibility.",
                Author = context.Accounts.First(),
                WorkoutSets = new List<WorkoutSet>()
    {
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 9) },
        new WorkoutSet() { Exercise = context.Exercises.FirstOrDefault(e => e.ExerciseId == 10) },
    }
            };
            context.Workouts.Add(workout10);

            context.SaveChanges();
























            var workoutSet = new WorkoutSet()
            {
                Exercise = context.Exercises.First(),
                Workout = context.Workouts.First(),

            };

            context.WorkoutSets.Add(workoutSet);
            context.SaveChanges();





            /*
            * Message Population 3 Pages
            */

            var message1 = new Message()
            {
                Content = "Hey, let's plan a workout together!",
                Sender = account1,
                Recipient = account2,
                DateSent = DateTime.Now.AddHours(-1),
            };

            var message2 = new Message()
            {
                Content = "Sure, I'm up for it. How about tomorrow?",
                Sender = account2,
                Recipient = account1,
                DateSent = DateTime.Now.AddHours(-0.5),
            };

            var message3 = new Message()
            {
                Content = "Sounds good! Let's do it!",
                Sender = account1,
                Recipient = account2,
                DateSent = DateTime.Now,
            };

            // Add Messages to DbSet
            context.Messages.AddRange(message1, message2, message3);

            context.SaveChanges();





            /*
            * TrackedWorkout Population 3 Pages
            */

            var trackedWorkout1 = new TrackedWorkout()
            {
                Account = account1,
                Workout = workout1,
                Completed = DateTime.Now.AddDays(-1),
            };

            var trackedWorkout2 = new TrackedWorkout()
            {
                Account = account2,
                Workout = workout2,
                Completed = DateTime.Now.AddDays(-2),
            };

            var trackedWorkout3 = new TrackedWorkout()
            {
                Account = account3,
                Workout = workout3,
                Completed = DateTime.Now.AddDays(-3),
            };

            context.TrackedWorkouts.AddRange(trackedWorkout1, trackedWorkout2, trackedWorkout3);

            context.SaveChanges();
        }
    }
}
