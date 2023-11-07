using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Controllers
{
    public class TrackingController : Controller
    {

        // Create a list of TrackedWorkoutDTO instances with placeholder properties
        List<TrackedWorkoutDTO> TrackedWorkouts = new List<TrackedWorkoutDTO>()
        {
            new TrackedWorkoutDTO
            {
                TrackedWorkoutID = 1,
                WorkoutID = 1,
                AccountID = 1,
                DateCompleted = DateTime.Now,
                Account = new AccountModel
                {
                    Name = "Name1",
                    Email = "exampleemail1@gmail.com"
                },
                Workout = new WorkoutModel
                {
                    Name = "Name1",
                }

            },
            new TrackedWorkoutDTO
            {
                TrackedWorkoutID = 2,
                WorkoutID = 2,
                AccountID = 12,
                DateCompleted = DateTime.Now,
                Account = new AccountModel
                {
                    Name = "Name1",
                    Email = "exampleemail2@gmail.com"
                },
                Workout = new WorkoutModel
                {
                    Name = "Name2",
                }
            },
        };

        // GET: TrackingController
        public ActionResult Index()
        {
            return View(TrackedWorkouts);
        }

        // GET: TrackingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrackingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrackingController/Create
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

        // GET: TrackingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrackingController/Edit/5
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

    }
}
