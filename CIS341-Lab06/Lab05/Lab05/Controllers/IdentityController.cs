using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Controllers
{
    public class IdentityController : Controller
    {
        // Create a list of RolesDTO instances with placeholder properties
        List<RoleDTO> Roles = new List<RoleDTO>()
        {
            new RoleDTO
            {
                RoleId = 1,
                Name = "name1",
            
            },
            new RoleDTO
            {
                RoleId = 2,
                Name = "name2",
            },
        };


        // GET: IdentityController
        public ActionResult Index()
        {
            return View(Roles);
        }

        // GET: IdentityController/Details/5
        public ActionResult Details(int id)
        {
            return View(Roles[0]);
        }

        // GET: IdentityController/Create
        public ActionResult Create()
        {
            return View(new RoleDTO());
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
            return View(Roles[0]);
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
            return View(Roles[0]);
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

        public ActionResult Login()
        {
            return View();
        }
    }
}
