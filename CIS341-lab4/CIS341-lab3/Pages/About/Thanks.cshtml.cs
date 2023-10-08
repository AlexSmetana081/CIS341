using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using System;

namespace CIS341_lab3.Pages.About
{
    [BindProperties]
    public class ThanksModel : PageModel
    {
        public string CurrentDateTime { get; private set; }
        private LinkGenerator _linkgen;

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private const string ContactPagePath = "/About/Contact";

        public ThanksModel(LinkGenerator linkgen)
        {
            _linkgen = linkgen; // Inject LinkGenerator through DI.
        }

        public void OnGet()
        {
            CurrentDateTime = DateTime.Now.ToString(DateTimeFormat);
        }

        public IActionResult OnPost()
        {
            // Handle form submission logic here
            return Redirect(_linkgen.GetPathByPage(ContactPagePath));
        }
    }
}
