using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Controllers
{
    /*
     * Identity Page is barebones. Need to update at a later time w/plans.
     */


    [Authorize]
    public class IdentityController : Controller
    {
        // GET: IdentityController
        public ActionResult Index()
        {
            return View();
        }

        // GET: IdentityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IdentityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IdentityController/Create
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

        // GET: IdentityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IdentityController/Edit/5
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

        // GET: IdentityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IdentityController/Delete/5
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

        //Returns view of login page
        public ActionResult Login()
        {
            return View();
        }
    }
}
