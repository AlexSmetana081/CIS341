using Lab09Smetana.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Lab09Smetana.Models;
using Lab09Smetana.ViewModels;

namespace Week12_WebAPI.Controllers
{
    /// <summary>
    /// API controller class for Exercises.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")] // URI base: /api/Exercises
    public class ExercisesController : ControllerBase
    {
        private readonly WorkoutContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">WorkoutContext DbContext object</param>
        /// <remarks>
        /// Injected automatically from services container.
        /// </remarks>
        public ExercisesController(WorkoutContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list with all to-do items.
        /// </summary>
        /// <response code="200">Returns a list with the available to-do items.</response>
        //[HttpGet(Name = "GetExercises")]
        [HttpGet("GetExercises")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<List<ExerciseDTO>>> GetAsync()
        {
            // Return items
            return await _context.Exercises.Select(t => new ExerciseDTO(t)).ToListAsync();
        }

        /// <summary>
        /// Get the specified to-do item.
        /// </summary>
        /// <response code="200">Returns the specified to-do item.</response>
        /// <response code="404">Specified to-do item not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json", "text/plain")]
        // Needed to get around a bug in the routing system - CreatedAtAction cannot otherwise find the correct action method by name.
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<ExerciseDTO>> GetAsync(int id)
        {
            var result = await _context.Exercises.Where(t => t.ExerciseId == id).FirstOrDefaultAsync();
            if (result != null)
                return new ExerciseDTO(result);
            else
            {
                return NotFound($"To-do item with the ID of {id} was not found.");
            }
        }

        /// <summary>
        /// Create a new to-do item.
        /// </summary>
        /// <response code="201">Returns the created to-do item.</response>
        /// <response code="400">Indicates that a validation error occurred.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(ExerciseDTO Exercise)
        {
            var exercise = new Exercise()
            {
                Name = Exercise.Name,
                Description = Exercise.Description,
                AccountId = Exercise.AccountId,
                WorkoutIntensity = Exercise.WorkoutIntensity
            };
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();

            // Model validation is done automatically and HTTP 400 (Bad Request) is automatically triggered.
            var actionName = nameof(GetAsync);
            var routeValues = new { id = Exercise.ExerciseId };
            return CreatedAtAction(actionName, routeValues, new ExerciseDTO(exercise));
        }

        /// <summary>
        /// Delete the specified to-do item.
        /// Note: Use of TypedResults means we do not need to specify ResponseType.
        /// </summary>
        /// <response code="204">Operation was successful.</response>
        /// <response code="404">Specified to-do item was not found.</response>
        [HttpDelete]
        [Produces("application/json")]
        public async Task<IResult> DeleteAsync(int id)
        {
            var result = await _context.Exercises.Where(t => t.ExerciseId == id).FirstOrDefaultAsync();
            if (result != null)
            {
                _context.Exercises.Remove(result);
                await _context.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound($"To-do item with the ID of {id} was not found.");
            }
        }
    }
}