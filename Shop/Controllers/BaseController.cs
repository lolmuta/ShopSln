using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Filter;
using Shop.Utils;

namespace Shop.Controllers
{
    [TypeFilter(typeof(CustomAuthorizeFilter))]
    public class BaseController : Controller
    {
    }
}
