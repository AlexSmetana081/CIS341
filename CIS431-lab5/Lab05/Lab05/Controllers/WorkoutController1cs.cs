using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Controllers
{
    public class WorkoutController1cs : Controller
    {
        // GET: WorkoutController1cs
        public ActionResult Index()
        {
            return View();
        }

        // GET: WorkoutController1cs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkoutController1cs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkoutController1cs/Create
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

        // GET: WorkoutController1cs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkoutController1cs/Edit/5
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

        // GET: WorkoutController1cs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkoutController1cs/Delete/5
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
