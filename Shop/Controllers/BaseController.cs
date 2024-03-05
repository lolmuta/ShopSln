using Microsoft.AspNetCore.Mvc;
using Shop.Filter;

namespace Shop.Controllers
{
    [TypeFilter(typeof(CustomAuthorizeFilter))]
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
