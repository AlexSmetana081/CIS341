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
        public async Task<IActionResult> Index()
        {
            return _context.Workouts != null ?
                        View(await _context.Workouts.Include(a => a.Exercises).ToListAsync()) :
                        Problem("Entity set 'FitnessContext.Workouts'  is null.");
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var Workouts = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (Workouts == null)
            {
                return NotFound();
            }

            return View(Workouts);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            var accounts = _context.Accounts.ToList();
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

            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,AuthorId,WorkoutSet")] WorkoutDTO workout)
        {
            if (ModelState.IsValid)
            {
                var workoutSets = _context.WorkoutSets.Where(a => workout.Exercises.Contains(a.WorkoutSetId))?.ToList();
                _context.Add(new Workout()
                {
                    Name = workout.Name,
                    Description = workout.Description,
                    AuthorId= workout.AuthorId,
                    Exercises= workoutSets,
                });


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(workout);
        }

        // GET: Workouts/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("WorkoutId,Name,Description,AuthorId")] WorkoutDTO workout)
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
                        var workoutSets = _context.WorkoutSets.Include(a => a.Workout).Include(a => a.Exercise).ToList();
                        /* Update changes to the entity and since it's being tracked
                         * as we are in a connected state, only SaveChangesAsync is needed
                         * to persist. */

                        entity.Name = workout.Name;
                        entity.AuthorId = workout.AuthorId;
                        entity.Description = workout.Description;
                        entity.Exercises = workoutSets;
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

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);

            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.WorkoutId == id);
        }



    }
}
