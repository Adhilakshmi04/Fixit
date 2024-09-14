using System.Diagnostics;
using FixitUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FixitUI.Controllers
{
    public class LoginController : Controller
    {
     
       public IActionResult Login(){

            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
