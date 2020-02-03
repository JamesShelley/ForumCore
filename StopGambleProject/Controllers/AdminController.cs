using Microsoft.AspNetCore.Mvc;
using StopGambleProject.Data;

namespace StopGambleProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}