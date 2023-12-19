using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Lab05.Controllers
{
    public class TrackingController : Controller
    {
        //Define Workout Context from database
        private readonly WorkoutContext _context;
        public TrackingController(WorkoutContext context)
        {
            _context = context;
        }

        // GET: TrackingController
        public async Task<IActionResult> Index()
        {
            //Returns view on index page if != null
            return _context.TrackedWorkouts != null ?
             View(await _context.TrackedWorkouts
             .Include(a => a.Account)
             .Include(a => a.Workout)
                 .ThenInclude(a => a.WorkoutSets)
                 .ThenInclude(a => a.Exercise)
             .ToListAsync()) :
             Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: TrackingController/Workout/1
        [Route("Tracking/Workout/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            //Returns view on index page if != null
            return _context.TrackedWorkouts != null ?
             View(await _context.TrackedWorkouts
             .Where(a => a.WorkoutId == id)
             .Include(a => a.Account)
             .Include(a => a.Workout)
                 .ThenInclude(a => a.WorkoutSets)
                 .ThenInclude(a => a.Exercise)
             .ToListAsync()) :
             Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: TrackingController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Retrive from context + LINQ Join
            var trackedWorkoutEntity = await _context.TrackedWorkouts
                .Include(a => a.Account)
                .Include(a => a.Workout)
                    .ThenInclude(a => a.WorkoutSets)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TrackedWorkoutId == id);

            //Return Not Found if TrackedWorkout is null
            if (trackedWorkoutEntity == null)
            {
                return NotFound();
            }

            //Return view if successful
            return View(trackedWorkoutEntity);
        }


        // GET: TrackingController/Create

        public IActionResult Create()
        {
            var workouts = _context.Workouts.ToList();
            var workoutList = workouts.Select(s => new SelectListItem { Value = s.WorkoutId.ToString(), Text = s.Name }).ToList();

            ViewBag.WorkoutList = workoutList;

            // Create a list with the currently logged-in user's account
            var loggedInUserName = User.Identity.Name;
            var loggedInAccount = _context.Accounts.FirstOrDefault(a => a.Name == loggedInUserName);

            if (loggedInAccount != null)
            {
                ViewBag.AccountList = new List<SelectListItem>
        {
            new SelectListItem { Value = loggedInAccount.AccountId.ToString(), Text = loggedInAccount.Name }
        };
            }
            else
            {
                ViewBag.AccountList = new List<SelectListItem>();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,WorkoutId")] TrackedWorkoutDTO trackedWorkout)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    trackedWorkout.Completed = DateTime.Now; // Set Completed to current time

                    _context.Add(new TrackedWorkout()
                    {
                        AccountId = trackedWorkout.AccountId,
                        WorkoutId = trackedWorkout.WorkoutId,
                        Completed = trackedWorkout.Completed
                    });

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log).
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            // Repopulate ViewBag lists if needed
            var workouts = _context.Workouts.ToList();
            var workoutList = new List<SelectListItem>();
            foreach (Workout s in workouts)
            {
                workoutList.Add(new SelectListItem { Value = s.WorkoutId.ToString(), Text = s.Name });
            }
            ViewBag.WorkoutList = workoutList;

            var accounts = _context.Accounts.ToList();
            var accountList = new List<SelectListItem>();
            foreach (Account s in accounts)
            {
                accountList.Add(new SelectListItem { Value = s.AccountId.ToString(), Text = s.Name });
            }
            ViewBag.AccountList = accountList;

            return View(trackedWorkout);
        }

        // GET: TrackingController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TrackedWorkouts == null)
            {
                return NotFound();
            }

            var trackedWorkouts = await _context.TrackedWorkouts
            .FirstOrDefaultAsync(l => l.TrackedWorkoutId == id);
            if (trackedWorkouts == null)
            {
                return NotFound();
            }

            // Use DTO to pass data to the View.
            TrackedWorkoutDTO trackedWorkoutDTO = new()
            {
                TrackedWorkoutId = trackedWorkouts.TrackedWorkoutId,
                AccountId = trackedWorkouts.AccountId,
                WorkoutId = trackedWorkouts.WorkoutId,
                Completed = trackedWorkouts.Completed
            };

            var workouts = _context.Workouts.ToList();
            var workoutList = new List<SelectListItem>();

            foreach (Workout s in workouts)
            {
                workoutList.Add(new SelectListItem { Value = s.WorkoutId.ToString(), Text = s.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.WorkoutList = workoutList;

            var accounts = _context.Accounts.ToList();
            var accountList = new List<SelectListItem>();


            foreach (Account s in accounts)
            {
                accountList.Add(new SelectListItem { Value = s.AccountId.ToString(), Text = s.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.AccountList = accountList;

            return View(trackedWorkoutDTO);
        }

        // POST: TrackingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrackedWorkoutId,AccountId,WorkoutId,Completed")] TrackedWorkoutDTO trackedWorkout)
        {
            if (id != trackedWorkout.TrackedWorkoutId)
            {
                return NotFound();
            }
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    // Pull entity from DB with the provided ID
                    var entity = await _context.TrackedWorkouts
                        .FirstOrDefaultAsync(l => l.TrackedWorkoutId == id);

                    if (entity != null)
                    {
                        /* Update changes to the entity and since it's being tracked
                         * as we are in a connected state, only SaveChangesAsync is needed
                         * to persist. */
                        entity.TrackedWorkoutId = trackedWorkout.TrackedWorkoutId;
                        entity.AccountId = trackedWorkout.AccountId;
                        entity.WorkoutId = trackedWorkout.WorkoutId;
                        entity.Completed = trackedWorkout.Completed;

                        // Save changes
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackedWorkoutExists(trackedWorkout.TrackedWorkoutId))
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
            return View(trackedWorkout);
        }

        // GET: TrackingController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackedWorkoutEntity = await _context.TrackedWorkouts
                .Include(a => a.Account)
                .Include(a => a.Workout)
                .FirstOrDefaultAsync(m => m.TrackedWorkoutId == id);

            if (trackedWorkoutEntity == null)
            {
                return NotFound();
            }

            return View(trackedWorkoutEntity);
        }

        // POST: TrackingController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trackedWorkout = await _context.TrackedWorkouts.FindAsync(id);
            if (trackedWorkout == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.TrackedWorkouts.Remove(trackedWorkout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
        public ActionResult Progress()
        {
            return View();
        }

        //Method for checking if TrackedWorkoutExists exists
        //Implementation in Edit
        private bool TrackedWorkoutExists(int id)
        {
            return _context.TrackedWorkouts.Any(e => e.TrackedWorkoutId == id);
        }

    }
}
