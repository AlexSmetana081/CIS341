using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lab05.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly WorkoutContext _context;
        public WorkoutController(WorkoutContext context)
        {
            _context = context;
        }

        // GET: Workouts
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var exercises = _context.Exercises.ToList();
            var exerciseList = new List<SelectListItem>();

            foreach (Exercise s in exercises)
            {
                exerciseList.Add(new SelectListItem { Value = s.AccountId.ToString(), Text = s.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.ExerciseList = exerciseList;

            return _context.Workouts != null ?
                        View(await _context.Workouts
                            .Include(a => a.Author)
                            .Include(a => a.WorkoutSets).ToListAsync()) :
                        Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: Workouts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            //Return notFound if Id is null
            if (id == null)
            {
                return NotFound();
            }

            //Retrieve from context + LINQ Join
            var workoutEntity = await _context.Workouts
                .Include(a => a.WorkoutSets)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.WorkoutId == id);

            //Return notFound if null
            if (workoutEntity == null)
            {
                return NotFound();
            }

            // Add list to entity.
            var workoutSets = _context.WorkoutSets
                .Where(a => a.WorkoutId == workoutEntity.WorkoutId)
                .Include(a => a.Workout)
                .Include(a => a.Exercise).ToList();

            workoutEntity.WorkoutSets = workoutSets;

            return View(workoutEntity);
        }

        // GET: Workouts/Create
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


        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Create([Bind("Name,Description,AuthorId,Exercises")] WorkoutDTO workout)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var workoutSets = _context.WorkoutSets
                        .Where(a => workout.Exercises.Contains(a.WorkoutSetId))?
                        .Select(a => new WorkoutSet() { ExerciseId = a.ExerciseId, WorkoutId = a.WorkoutId })
                        .ToList();
                    
                    _context.Add(new Workout()
                    {
                        Name = workout.Name,
                        Description = workout.Description,
                        AuthorId = workout.AuthorId,
                        WorkoutSets = workoutSets,
                        
                    });

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }


            return View(workout);
        }

        // GET: Workouts/Edit/5
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var Workouts = await _context.Workouts
            .FirstOrDefaultAsync(l => l.WorkoutId == id);

            if (Workouts == null)
            {
                return NotFound();
            }

            // Use DTO to pass data to the View.
            WorkoutDTO workoutDTO = new()
            {
                WorkoutId = Workouts.WorkoutId,
                AuthorId = Workouts.AuthorId,
                Name = Workouts.Name,
                Description = Workouts.Description,
            };

            workoutDTO.Exercises = _context.WorkoutSets
                .Where(a => a.WorkoutId == Workouts.WorkoutId)
                .Include(a => a.Workout)
                .Include(a => a.Exercise)
                .Select(a => a.WorkoutId)
                .ToList();

            var accounts = await _context.Accounts.ToListAsync();
            var accountList = new List<SelectListItem>();

            foreach (Account s in accounts)
            {
                accountList.Add(new SelectListItem { Value = s.AccountId.ToString(), Text = s.Name });
            }
            // Add list to ViewBag as a dynamic property.
            ViewBag.AccountList = accountList;

            var workoutSets = _context.WorkoutSets.Include(a => a.Workout).Include(a => a.Exercise).ToList();
            var workoutSetList = new List<SelectListItem>();


            foreach (WorkoutSet s in workoutSets)
            {
                workoutSetList.Add(new SelectListItem { Value = s.WorkoutSetId.ToString(), Text = s.Exercise.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.WorkoutSetList = workoutSetList;

            return View(workoutDTO);
        }

        // POST: Workouts/Edit/5
        /* Bind to DTO rather than the entity to avoid issues with navigation properties
         * and this is generally the preferable approach to avoid overposting. */
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Edit(int id, [Bind("WorkoutId,Name,Description,AuthorId,Exercises")] WorkoutDTO workout)
        {
            if (id != workout.WorkoutId)
            {
                return NotFound();
            }
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    // Pull entity from DB with the provided ID
                    var entity = await _context.Workouts
                        .FirstOrDefaultAsync(l => l.WorkoutId == id);

                    if (entity != null)
                    {
                        var workoutSets = _context.WorkoutSets
                            .Where(a => workout.Exercises.Contains(a.WorkoutSetId))?
                            .Select(a => new WorkoutSet() { ExerciseId = a.ExerciseId, WorkoutId = a.WorkoutId })
                            .ToList();
                        /* Update changes to the entity and since it's being tracked
                         * as we are in a connected state, only SaveChangesAsync is needed
                         * to persist. */
                        _context.RemoveRange(_context.WorkoutSets
                            .Where(a => a.WorkoutId == entity.WorkoutId));
                        entity.Name = workout.Name;
                        entity.AuthorId = workout.AuthorId;
                        entity.Description = workout.Description;
                        entity.WorkoutSets = workoutSets;

                        // Save changes
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.WorkoutId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workout);
        }

        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(workout);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        [Authorize(Roles = "Admin, Trainer, Subscriber")]
        public async Task<IActionResult> AddWorkout(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Workout ID is required.";
                return NotFound("Workout ID is required.");
            }

            // Get the currently logged-in user's name
            var loggedInUserName = User.Identity.Name;

            // Get the corresponding account from the context based on the username
            var loggedInAccount = _context.Accounts.FirstOrDefault(a => a.Name == loggedInUserName);

            if (loggedInAccount == null)
            {
                TempData["ErrorMessage"] = "User account not found.";
                return NotFound("User account not found.");
            }

            // Validate if the workout with the given ID exists
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                TempData["ErrorMessage"] = "Workout not found.";
                return NotFound("Workout not found.");
            }

            _context.Add(new TrackedWorkout()
            {
                AccountId = loggedInAccount.AccountId,  // Use the logged-in user's account ID
                WorkoutId = (int)id,
                Completed = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your workout was tracked.";
            return RedirectToAction(nameof(Index));
        }


        //Method for checking if Workout exists
        //Implementation in Edit
        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.WorkoutId == id);
        }



    }
}
