using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab05.Data;
using Lab05.Models;
using Lab05.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks.Dataflow;

namespace Lab05.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        //Define Workout Context from database
        private readonly WorkoutContext _context;

        public AccountsController(WorkoutContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            //Returns view on index page if != null
            return _context.Accounts != null ?
                        View(await _context.Accounts.ToListAsync()) :
                        Problem("Entity set 'FitnessContext.Accounts'  is null.");
        }

        // GET: Accounts/Details
        public async Task<IActionResult> Details(int? id)
        {
            //Return not found if ID + Accounts is null
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            //Retrive from context + LINQ Join
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AccountId == id);

            //Return not found if account is null
            if (account == null)
            {
                return NotFound();
            }

            //Returns view of account
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] AccountDTO account)
        {
            //Check if valid
            if (ModelState.IsValid)
            {
                //Add to database if valid
                _context.Add(new Account()
                {
                    Name = account.Name
                });

                //Saves and returns the view
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            //Returns not found if id or Accounts is null
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            //Join account to Id
            var account = await _context.Accounts.FindAsync(id);

            //Returns null if account is not found
            if (account == null)
            {
                return NotFound();
            }

            //Returns view if successful
            return View(account);
        }

        // POST: Accounts/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Name")] Account account)
        {
            //Returns not found if id is null
            if (id != account.AccountId)
            {
                return NotFound();
            }

            //Check if model state is valid
            if (ModelState.IsValid)
            {
                //Update and save changes if successful
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }

                //If not throw exception/return error!
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Return to index page
                return RedirectToAction(nameof(Index));
            }

            //Return view of account
            return View(account);
        }

        // GET: Accounts/Delete
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            //Returns not found if Id is null
            if (id == null)
            {
                return NotFound();
            }

            //LINQ join retrieve from database
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AccountId == id);

            //Returns not found if AccoundId is null
            if (account == null)
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
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //Delete Confirmed Function - Prompts a user if they want to delete the Account.
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Defines account context with Id
            var account = await _context.Accounts.FindAsync(id);

            //Returns home if account is null
            if (account == null)
            {
                return RedirectToAction(nameof(Index));
            }

            //Deletes the account if successful
            try
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                //Log the error if error occurs
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        //Method for checking if account exists
        //Implementation in Edit
        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }


    }
}