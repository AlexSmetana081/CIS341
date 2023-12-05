using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            //Returns view on index page if != null
            var exercises = await _context.Exercises.Include(e => e.AuthorName).ToListAsync();
            return View(exercises);
        }

        // GET: ExerciseController/Details/5
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
            //Returns not found if Id is null
            if (id == null)
            {
                return NotFound();
            }

            //LINQ join retrieve from database
            var exercise = await _context.Exercises.FindAsync(id);

            //Returns not found if Exercise is null
            if (exercise == null)
            {
                return NotFound();
            }

            // Convert the Exercise entity to ExerciseDTO
            var exerciseDTO = new ExerciseDTO
            {
                Name = exercise.Name,
                Description = exercise.Description,
                WorkoutIntensity = exercise.WorkoutIntensity
            };

            //Return view
            return View(exerciseDTO);
        }

        // POST: ExerciseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,AuthorId,WorkoutIntensity")] ExerciseDTO exerciseDTO)
        {
            //Returns not found if id is null
            if (id != exerciseDTO.ExerciseId)
            {
                return NotFound();
            }

            //Check if model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing account from the database based on the provided Id
                    var existingAccount = await _context.Accounts.FindAsync(exerciseDTO.ExerciseId);

                    //Update and save changes if successful
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

                    //Properties from DTO to model
                    exerciseToUpdate.Name = exerciseDTO.Name;
                    exerciseToUpdate.Description = exerciseDTO.Description;
                    exerciseToUpdate.AuthorName = existingAccount;
                    exerciseToUpdate.WorkoutIntensity = exerciseDTO.WorkoutIntensity;

                    //Updates the database
                    _context.Update(exerciseToUpdate);
                    await _context.SaveChangesAsync();
                }

                //Throws Exception if neccessary
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                //Returns to index
                return RedirectToAction(nameof(Index));
            }

            //Returns the view
            return View(exerciseDTO);
        }

        //GET: Exercise/Delete
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
        private bool ExerciseExists(int id)
        {
            return (_context.Exercises?.Any(e => e.ExerciseId == id)).GetValueOrDefault();
        }
    }
}
