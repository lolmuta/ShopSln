using Dapper;
using Microsoft.AspNetCore.Mvc;
using Shop.Utils;

namespace Shop.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbHelper dbHelper;
        private readonly UserInfoHelper userInfoHelper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(ILogger<HomeController> logger
            , DbHelper dbHelper
            , UserInfoHelper userInfoHelper
            , IHttpContextAccessor _httpContextAccessor)
        {
            _logger = logger;
            this.dbHelper = dbHelper;
            this.userInfoHelper = userInfoHelper;
            httpContextAccessor = _httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DoLogin([FromBody] UserIDPwd userIDPwd)
        {
            try
            {
                var sql = @"select 1 from Users where UserId = @UserId";
                var isValid = dbHelper.ConnDb(conn => conn.Query(sql, userIDPwd)).Count() > 0;
                if (isValid)
                {
                    userInfoHelper.SetUserId(userIDPwd.UserId);
                    return Ok(new { success = true, message = "登入成功" });
                }
                else
                {
                    return Ok(new { success = false, message = "登入失敗" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "測試");
                return BadRequest(new { success = false, message = "exception " + ex.Message });
            }
        }
    }
    public class UserIDPwd
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

}
