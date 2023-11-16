using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index()
        {
            return View();
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
