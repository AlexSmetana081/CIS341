using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab05.Controllers
{
    public class ContactController : Controller
    {

        // Create a list of MessageDTO instances with placeholder properties
        List<MessageDTO> Messaage = new List<MessageDTO>()
        {
            new MessageDTO
            {
                MessageId = 1,
                RecipientId = 1,
                SenderId= 1,
                Content = "1",
                Recipient = new AccountModel
                {
                    Name = "Name1",
                    Email = "exampleemail1@gmail.com"
                },
                Sender = new AccountModel
                {
                    Name = "Name1",
                    Email = "exampleemail1@gmail.com"
                }
            },
             new MessageDTO
            {
                MessageId = 2,
                RecipientId = 2,
                SenderId= 2,
                Content = "2",
                Recipient = new AccountModel
                {
                    Name = "Name2",
                    Email = "exampleemail2@gmail.com"
                },
                Sender = new AccountModel
                {
                    Name = "Name2",
                    Email = "exampleemail2@gmail.com"
                }
            }
        };

        // GET: Contact
        public ActionResult Index()
        {
            return View(Messaage);
        }

        // GET: Contact/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
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

        // GET: Contact/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contact/Edit/5
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

        // GET: Contact/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contact/Delete/5
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


        public ActionResult Trainer(int id)
        {
            return View();
        }
    }
}
