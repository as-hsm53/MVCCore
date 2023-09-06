using Microsoft.AspNetCore.Mvc;
using MyFirstMVC.Models;
using System.Diagnostics;

namespace MyFirstMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IEmailSender emailSender;
        //IEmailSender emailSender
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //this.emailSender = emailSender;
        }

        //public Task<IActionResult> Index()
        //{
            //var email = "Hussain.khozema@jsil.com";
            //var subject = "Test";
            //var message = "Sheeesh";

            //await emailSender.SendEmailAsync(email, subject, message);
            //return View();
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}