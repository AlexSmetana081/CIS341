/*
    Author: Alex Smetana
    CIS341
    09/28/2023
*/


using CIS341_lab3.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace CIS341_lab3.Pages
{
    // Bind POST requests
    [BindProperties]
    public class ContactModel : PageModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }

        //Placeholders for text
        //Probably a more efficent way to do this. 
        public const string? NamePlaceholder = "Enter your Name here.";
        public const string? EmailPlaceholder = "Enter your Email here.";
        public const string? MessagePlaceholder = "Enter your Message here.";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            //Check model is valid
            if (ModelState.IsValid)
            {
                // Create a new Contact object
                var contact = new Contact
                {
                    Name = Name,
                    Email = Email,
                    Message = Message
                };

                Console.WriteLine($"Name: {contact.Name}");
                Console.WriteLine($"Email: {contact.Email}");
                Console.WriteLine($"Message: {contact.Message}");

                //Redirct to Index
                return RedirectToPage("Index");
            }

            //Return if model is not valid
            return Page();
        }
        

    }
}
