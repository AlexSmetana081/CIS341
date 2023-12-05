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
    public class WorkoutSetController : Controller
    {
        private readonly WorkoutContext _context;

        public WorkoutSetController(WorkoutContext context)
        {
            _context = context;
        }
        // GET: WorkoutSetController
        public async Task<IActionResult> Index()
        {
            //Returns view on index page if != null
            return _context.WorkoutSets != null ?
                        View(await _context.WorkoutSets
                            .Include(a => a.Exercise)
                            .Include(a => a.Workout).ToListAsync()) :
                        Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: WorkoutSetController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Return notFound if Id is null
            if (id == null)
            {
                return NotFound();
            }

            //Retrive from context + LINQ Join
            var workoutSetEntity = await _context.WorkoutSets
                .Include(a => a.Exercise)
                .Include(a => a.Workout)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.WorkoutId == id);

            //Return notFound if null
            if (workoutSetEntity == null)
            {
                return NotFound();
            }

            //Return view of workoutset
            return View(workoutSetEntity);
        }

        // GET: WorkoutSetController/Create
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

            var exercises = _context.Exercises.ToList();
            var exerciseList = new List<SelectListItem>();


            foreach (Exercise s in exercises)
            {
                exerciseList.Add(new SelectListItem { Value = s.ExerciseId.ToString(), Text = s.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.ExerciseList = exerciseList;

            return View();
        }

        // POST: WorkoutSetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciseId,WorkoutId")] WorkoutSetDTO workout)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(new WorkoutSet()
                    {
                        ExerciseId = workout.ExerciseId,
                        WorkoutId = workout.WorkoutId,
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

        // GET: WorkoutSetController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkoutSets == null)
            {
                return NotFound();
            }

            var WorkoutSets = await _context.WorkoutSets
            .FirstOrDefaultAsync(l => l.WorkoutSetId == id);
            if (WorkoutSets == null)
            {
                return NotFound();
            }

            // Use DTO to pass data to the View.
            WorkoutSetDTO workoutSetDTO = new()
            {
                ExerciseId = WorkoutSets.ExerciseId,
                WorkoutId = WorkoutSets.WorkoutId,
                WorkoutSetId = WorkoutSets.WorkoutSetId
            };

            var workouts = _context.Workouts.ToList();
            var workoutList = new List<SelectListItem>();

            foreach (Workout s in workouts)
            {
                workoutList.Add(new SelectListItem { Value = s.WorkoutId.ToString(), Text = s.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.WorkoutList = workoutList;

            var exercises = _context.Exercises.ToList();
            var exerciseList = new List<SelectListItem>();


            foreach (Exercise s in exercises)
            {
                exerciseList.Add(new SelectListItem { Value = s.ExerciseId.ToString(), Text = s.Name });
            }

            // Add list to ViewBag as a dynamic property.
            ViewBag.ExerciseList = exerciseList;

            return View(workoutSetDTO);
        }

        // POST: WorkoutSetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkoutSetId,ExerciseId,WorkoutId")] WorkoutSetDTO workoutSet)
        {
            if (id != workoutSet.WorkoutId)
            {
                return NotFound();
            }
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    // Pull entity from DB with the provided ID
                    var entity = await _context.WorkoutSets
                        .FirstOrDefaultAsync(l => l.WorkoutSetId == id);

                    if (entity != null)
                    {
                        /* Update changes to the entity and since it's being tracked
                         * as we are in a connected state, only SaveChangesAsync is needed
                         * to persist. */
                        entity.WorkoutSetId = workoutSet.WorkoutSetId;
                        entity.ExerciseId = workoutSet.ExerciseId;
                        entity.WorkoutId = workoutSet.WorkoutId;
                        // Save changes
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutSetExists(workoutSet.WorkoutSetId))
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
            return View(workoutSet);
        }

        // GET: WorkoutSetController/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            //Returns not found if Id is null
            if (id == null)
            {
                return NotFound();
            }

            //LINQ join
            var workoutSet = await _context.WorkoutSets
                .FirstOrDefaultAsync(m => m.WorkoutSetId == id);

            if (workoutSet == null)
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
            return View(workoutSet);
        }

        // POST: WorkoutSetController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //Delete Confirmed Function - Prompts a user if they want to delete the WorkoutSet.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Defines workoutSet context with Id
            var workoutSet = await _context.WorkoutSets.FindAsync(id);

            //Returns home if account is null
            if (workoutSet == null)
            {
                return NotFound();
            }

            //Deletes the account if successful
            _context.WorkoutSets.Remove(workoutSet);
            await _context.SaveChangesAsync();

            //Returns to home after deletion
            return RedirectToAction(nameof(Index));
        }

        //Method for checking if WorkoutSet exists
        //Implementation in Edit
        private bool WorkoutSetExists(int id)
        {
            return _context.WorkoutSets.Any(e => e.WorkoutSetId == id);
        }

    }
}
