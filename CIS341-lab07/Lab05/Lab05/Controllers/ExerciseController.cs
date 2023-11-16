using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly WorkoutContext _context;

        public ExerciseController(WorkoutContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var exercises = await _context.Exercises.Include(e => e.AuthorName).ToListAsync();
            return View(exercises);
        }

        // GET: ExerciseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExerciseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExerciseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,AuthorName,WorkoutIntensity")] ExerciseDTO exercise)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing account from the database based on the provided name
                var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == exercise.AuthorName.Name);

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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the exercise from the database
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            // Convert the Exercise entity to ExerciseDTO for editing
            var exerciseDTO = new ExerciseDTO
            {
                Name = exercise.Name,
                Description = exercise.Description,
                //AuthorName = exercise.AuthorName.AccountId, 
                WorkoutIntensity = exercise.WorkoutIntensity
            };

            return View(exerciseDTO);
        }

        // POST: ExerciseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,AuthorId,WorkoutIntensity")] ExerciseDTO exerciseDTO)
        {
            if (id != exerciseDTO.ExerciseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing account from the database based on the provided Id
                    var existingAccount = await _context.Accounts.FindAsync(exerciseDTO.ExerciseId);

                    // If the account doesn't exist, handle this situation accordingly
                    if (existingAccount == null)
                    {
                        ModelState.AddModelError("AuthorId", "Account not found");
                        return View(exerciseDTO);
                    }

                    // Update the Exercise entity with the edited properties
                    var exerciseToUpdate = await _context.Exercises.FindAsync(id);
                    if (exerciseToUpdate == null)
                    {
                        return NotFound();
                    }

                    exerciseToUpdate.Name = exerciseDTO.Name;
                    exerciseToUpdate.Description = exerciseDTO.Description;
                    exerciseToUpdate.AuthorName = existingAccount; // Use the existing account
                    exerciseToUpdate.WorkoutIntensity = exerciseDTO.WorkoutIntensity;

                    _context.Update(exerciseToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency issues if necessary
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(exerciseDTO);
        }

        // GET: ExerciseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExerciseController/Delete/5
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
    }
}
