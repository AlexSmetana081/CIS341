/*
    Author: Alex Smetana
    CIS341
    09/28/2023
*/

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace CIS341_lab3.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //Changes made by tutorial
            //https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-aspnet-core?view=vs-2022
            string dateTime = DateTime.Now.ToString("d", new CultureInfo("en-US"));
            ViewData["TimeStamp"] = dateTime;
        }

    }
}