using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Lab05.Controllers
{
    public class TrackingController : Controller
    {
        private readonly WorkoutContext _context;
        public TrackingController(WorkoutContext context)
        {
            _context = context;
        }


        // GET: TrackingController
        public async Task<IActionResult> Index()
        {
            return _context.TrackedWorkouts != null ?
             View(await _context.TrackedWorkouts
             .Include(a => a.Workout)
             .Include(a => a.Account).ToListAsync()) :
             Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: TrackingController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the trackedWorkoutEntity from the repository or context
            var trackedWorkoutEntity = await _context.TrackedWorkouts
                .Include(a => a.Account)
                .Include(a => a.Workout)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TrackedWorkoutId == id);


            if (trackedWorkoutEntity == null)
            {
                return NotFound();
            }

            return View(trackedWorkoutEntity);
        }


        // GET: TrackingController/Create
        public IActionResult Create()
        {
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

            return View();
        }

        // POST: TrackingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,WorkoutId,Completed")] TrackedWorkoutDTO trackedWorkout)
        {
            try
            {
                if (ModelState.IsValid)
                {
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
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }


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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrackingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Progress()
        {
            return View();
        }

        private bool TrackedWorkoutExists(int id)
        {
            return _context.TrackedWorkouts.Any(e => e.TrackedWorkoutId == id);
        }

    }
}
