using Microsoft.AspNetCore.Mvc;

namespace BookStack_Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
