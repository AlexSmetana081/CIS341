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
            return _context.WorkoutSets != null ?
                        View(await _context.WorkoutSets
                            .Include(a => a.Exercise)
                            .Include(a => a.Workout).ToListAsync()) :
                        Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: WorkoutSetController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the workoutset from the repository or context
            var workoutSetEntity = await _context.WorkoutSets
                .Include(a => a.Exercise)
                .Include(a => a.Workout)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.WorkoutId == id);


            if (workoutSetEntity == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("ExerciseId,WorkoutId,Sets,Reps,Weight")] WorkoutSetDTO workout)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(new WorkoutSet()
                    {
                        ExerciseId = workout.ExerciseId,
                        WorkoutId = workout.WorkoutId,
                        Sets = workout.Sets,
                        Reps = workout.Reps,
                        Weight = workout.Weight,
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutSet = await _context.WorkoutSets
                .FirstOrDefaultAsync(m => m.WorkoutSetId == id);

            if (workoutSet == null)
            {
                return NotFound();
            }

            return View(workoutSet);
        }

        // POST: WorkoutSetController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutSet = await _context.WorkoutSets.FindAsync(id);

            if (workoutSet == null)
            {
                return NotFound();
            }

            _context.WorkoutSets.Remove(workoutSet);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutSetExists(int id)
        {
            return _context.WorkoutSets.Any(e => e.WorkoutSetId == id);
        }

    }
}
