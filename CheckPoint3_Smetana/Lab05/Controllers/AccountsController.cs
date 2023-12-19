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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.DiaSymReader;

namespace Lab05.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        //Define Workout Context from database
        private readonly WorkoutContext _context;
        private readonly WorkoutAuthenticationContext _authenticationContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(WorkoutContext context, WorkoutAuthenticationContext authenticationContext, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _authenticationContext = authenticationContext;

        }


        // GET: Accounts
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var accounts = _context.Accounts.ToList();
            var users = _authenticationContext.Users.ToList();
            var userRoles = _authenticationContext.UserRoles.ToList();
            var roles = _authenticationContext.Roles.ToList();

            var leftOuterJoin =
                from a in accounts
                join u in users on a.Name equals u.Email into temp
                from last in temp.DefaultIfEmpty()
                select new UserAccountDTO
                {
                    Id = last == null ? string.Empty : last.Id,
                    AccountId = a.AccountId,
                    Name = a.Name,
                    Role = "Unknown",
                };

            var rightOuterJoin =
                from u in users
                join a in accounts on u.Email equals a.Name into temp
                from first in temp.DefaultIfEmpty()
                select new UserAccountDTO
                {
                    Id = u.Id,
                    AccountId = first == null ? -1 : first.AccountId,
                    Name = first == null ? string.Empty : first.Name,
                    Role = "Unknown",
                };

            var userAccountsBefore = leftOuterJoin.Union(rightOuterJoin);
            List<UserAccountDTO> userAccounts = new List<UserAccountDTO>();
            foreach (var acct in userAccountsBefore)
            {
                if (!userAccounts
                    .Where(a => a.Id == acct.Id)
                    .Where(a => a.AccountId == acct.AccountId)
                    .Where(a => a.Name.Equals(acct.Name)).Any())
                {
                    var userRole = userRoles.FirstOrDefault(a => a.UserId == acct.Id);
                    var role = roles.Where(a => a.Id == userRole?.RoleId).FirstOrDefault();
                    userAccounts.Add(acct);
                    if (role != null)
                    {
                        userAccounts.Last().Role = role.Name;
                    }
                }
            }

            //Returns view on index page if != null
            return userAccounts != null ?
                        View(userAccounts.Where(a => a.AccountId >= 0).OrderBy(a => a.Id).ToList()) :
                        Problem("Entity set 'FitnessContext.Users'  is null.");
        }

        // GET: Accounts/Details
        [Authorize(Roles = "Admin, Trainer, Subscriber")]
        public async Task<IActionResult> Details(int? id)
        {
            //Return not found if ID + Accounts is null
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            //Retrieve from context + LINQ Join
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AccountId == id);

            //Return not found if account is null
            if (account == null)
            {
                return NotFound();
            }

            //Retrieve user from context + LINQ Join
            var user = await _authenticationContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Email.Equals(account.Name));

            UserAccountDTO accountDTO = new UserAccountDTO()
            {
                Id = user == null ? string.Empty : user.Id,
                AccountId = account == null ? -1 : account.AccountId,
                Name = account == null ? string.Empty : account.Name,
                Role = "tbd",
            };

            //Returns view of account
            return View(accountDTO);
        }

        // GET: Accounts/Create
        [Authorize(Roles = "Admin, Trainer")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
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
        [Authorize(Roles = "Admin, Trainer")]
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
        [Authorize(Roles = "Admin, Trainer")]
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
        [Authorize(Roles = "Admin, Trainer")]
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
        [Authorize(Roles = "Admin, Trainer")]
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