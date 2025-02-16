using Microsoft.AspNetCore.Mvc;

namespace GitHub_API.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
