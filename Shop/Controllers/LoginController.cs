using Dapper;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
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
        [HttpPost]
        public IActionResult DoLogin([FromBody] UserIDPwd userIDPwd)
        {
            try
            {
                var sql = @"
                    SELECT 
                      [UserId]
                      ,[UserName]
                      ,[Addr] 
                    from 
                        Users 
                    where UserId = @UserId";

                var users = dbHelper.ConnDb(conn => conn.Query<User>(sql, userIDPwd));
                var isValid = users.Count() > 0;    
                if (isValid)
                {
                    userInfoHelper.User = users.First();
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
        [HttpPost]
        public IActionResult DoLogout()
        {
            try
            {
                userInfoHelper.logOut();
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "測試");
                return View("Index");
            }
        }
    }
    public class UserIDPwd
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

}
