using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CIS341_lab3.Pages
{
    public class StatusCodeModel : PageModel
    {

        
        private readonly ILogger<StatusCodeModel> _logger;
        public string? ErrorMessage { get; set; }

        //Logging for logging 404 errors.
        public StatusCodeModel(ILogger<StatusCodeModel> logger)
        {
            _logger = logger;
        }

        //Inspired from:
        //10/06/2023
        //https://khalidabuhakmeh.com/handling-aspnet-core-exceptions-with-exceptionhandler-middleware

        public void OnGet(int? statusCode = null)
        {
            //Compare 404. Sends error and logs data.
            if (statusCode.HasValue)
            {

                if (HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    ErrorMessage = $"The requested path: {HttpContext.Request.GetDisplayUrl()} could not be found.";

                }
                else
                {
                    ErrorMessage = $"Status Code: {HttpContext.Response.StatusCode}";

                }

                //Log Status Code, Request Path, and User Agent Information.
                _logger.LogError($"Status Code: {HttpContext.Response.StatusCode} ");
                _logger.LogError($"Requested Path -  {HttpContext.Request.Path}");
                _logger.LogError($"User Agent: {HttpContext.Request.Headers["User-Agent"]}");
            }
        }


    }
}