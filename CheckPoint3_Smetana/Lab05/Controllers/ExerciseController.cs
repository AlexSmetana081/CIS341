using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        private readonly WorkoutContext _context;

        public ExerciseController(WorkoutContext context)
        {
            _context = context;
        }

        // GET: Exercises
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //Returns view on index page if != null
            var exercises = await _context.Exercises.Include(e => e.AuthorName).ToListAsync();
            return View(exercises);
        }

        // GET: ExerciseController/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Retrive from context + LINQ Join
            var exerciseEntity = await _context.Exercises.Include(e => e.AuthorName)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExerciseId == id);


            if (exerciseEntity == null)
            {
                return NotFound();
            }

            return View(exerciseEntity);
        }


        // GET: Workouts/Create
        [Authorize(Roles = "Admin, Trainer")]
        public IActionResult Create()
        {
            // Get the currently logged-in user's name
            var loggedInUserName = User.Identity.Name;

            // Get the corresponding account from the context based on the username
            var loggedInAccount = _context.Accounts.FirstOrDefault(a => a.Name == loggedInUserName);

            // If the account is found, create a list with a single SelectListItem
            // representing the logged-in account
            var accountList = loggedInAccount != null
                ? new List<SelectListItem>
                {
            new SelectListItem { Value = loggedInAccount.AccountId.ToString(), Text = loggedInAccount.Name }
                }
                : new List<SelectListItem>();

            // Add the account list to ViewBag
            ViewBag.AccountList = accountList;

            // Populate the workout set list
            var workoutSets = _context.WorkoutSets.Include(a => a.Workout).Include(a => a.Exercise).ToList();
            var workoutSetList = workoutSets.Select(s => new SelectListItem { Value = s.WorkoutSetId.ToString(), Text = s.Exercise.Name }).ToList();

            // Add the workout set list to ViewBag
            ViewBag.WorkoutSetList = workoutSetList;

            return View();
        }


        // POST: ExerciseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Create([Bind("Name,Description,AuthorNameId,WorkoutIntensity")] ExerciseDTO exercise)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing account from the database based on the provided name
                var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == exercise.AuthorNameId);

                // If the account doesn't exist, you may want to handle this situation accordingly
                if (existingAccount == null)
                {
                    ModelState.AddModelError("AuthorName.Name", "Account not found");
                    return View(exercise);
                }

                // Create a new Exercise and set its properties
                _context.Add(new Exercise()
                {
                    Name = exercise.Name,
                    Description = exercise.Description,
                    AuthorName = existingAccount, // Use the existing account
                    WorkoutIntensity = exercise.WorkoutIntensity

                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(exercise);
        }

        // GET: ExerciseController/Edit/5
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(a => a.AuthorName)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExerciseId == id);

            var accounts = await _context.Accounts.ToListAsync();
            var accountList = new List<SelectListItem>();

            foreach (Account s in accounts)
            {
                accountList.Add(new SelectListItem { Value = s.AccountId.ToString(), Text = s.Name });
            }
            // Add list to ViewBag as a dynamic property.
            ViewBag.AccountList = accountList;

            if (exercise == null)
            {
                return NotFound();
            }

            var exerciseDTO = new ExerciseDTO
            {
                ExerciseId = exercise.ExerciseId,
                Name = exercise.Name,
                Description = exercise.Description,
                AuthorNameId = exercise.AccountId, // Assuming AuthorName is the correct property for author
                WorkoutIntensity = exercise.WorkoutIntensity
            };

            return View(exerciseDTO);
        }

        // POST: ExerciseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseId,Name,Description,AuthorNameId,WorkoutIntensity")] ExerciseDTO exerciseDTO)
        {
            if (id != exerciseDTO.ExerciseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing exercise without AsNoTracking()
                    var existingExercise = await _context.Exercises.FindAsync(id);

                    if (existingExercise == null)
                    {
                        return NotFound();
                    }

                    // Update the Exercise entity with the edited properties
                    existingExercise.Name = exerciseDTO.Name;
                    existingExercise.Description = exerciseDTO.Description;
                    existingExercise.WorkoutIntensity = exerciseDTO.WorkoutIntensity;

                    // Note: Assuming AuthorName is the correct property for author
                    existingExercise.AccountId = exerciseDTO.AuthorNameId;


                    // Updates the database
                    _context.Update(existingExercise);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Log the exception or handle the concurrency conflict
                    ModelState.AddModelError("", "Concurrency error occurred. Please refresh and try again.");
                    return View(exerciseDTO);
                }
            }

            return View(exerciseDTO);
        }

        //GET: Exercise/Delete
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            //Returns not found if Id is null
            if (id == null)
            {
                return NotFound();
            }

            //LINQ join
            var exercise = await _context.Exercises
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ExerciseId == id);

            //Returns not found if exercise is null
            if (exercise == null)
            {
                return NotFound();
            }

            //Error message for delete
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            //Returns view if successful
            return View(exercise);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        //Delete Confirmed Function - Prompts a user if they want to delete the Exercise.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Defines exercise context with Id
            var exercise = await _context.Exercises.FindAsync(id);

            //Returns exercise if account is null
            if (exercise == null)
            {
                return RedirectToAction(nameof(Index));
            }

            //Deletes the Exercise if successful
            try
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                //Log the error if error occurs
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }


        //Method for checking if Exercise exists.
        //Implementation in Edit
        [Authorize(Roles = "Admin, Trainer")]
        private bool ExerciseExists(int id)
        {
            return (_context.Exercises?.Any(e => e.ExerciseId == id)).GetValueOrDefault();
        }
    }
}
