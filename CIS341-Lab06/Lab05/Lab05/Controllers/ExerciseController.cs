using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Controllers
{
    public class ExerciseController : Controller
    {
        // Create a list of ExerciseDTO instances with placeholder properties
        List<ExerciseDTO> Exercises = new List<ExerciseDTO>()
        {
            new ExerciseDTO
            {
                ExerciseId= 1,
                Name = "Name1",
                Description = "Description1",
                Length= 1,
                Intensity= 1,
                Author = new AccountModel
                {
                    Name = "Name1",
                    Email = "recipentemail1@gmail.com"
                }
            },
             new ExerciseDTO
            {
                ExerciseId= 2,
                Name = "Name2",
                Description = "Description2",
                Length= 2,
                Intensity= 2,
                Author = new AccountModel
                {
                    Name = "Name2",
                    Email = "recipentemail2@gmail.com"
                }
            }
        };


        // GET: ExerciseController
        public ActionResult Index()
        {
            return View(Exercises);
        }

        // GET: ExerciseController/Details/5
        public ActionResult Details(int id)
        {
            return View(Exercises[0]);
        }

        // GET: ExerciseController/Create
        public ActionResult Create()
        {
            return View(new ExerciseDTO());
        }

        // POST: ExerciseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ExerciseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExerciseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ExerciseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(Exercises[0]);
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
