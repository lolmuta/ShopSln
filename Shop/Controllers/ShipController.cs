using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Filter;
using Shop.Models;
using Shop.Utils;

namespace Shop.Controllers
{
    public class ShipController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbHelper dbHelper;
        private readonly UserInfoHelper userInfoHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ShipController(ILogger<HomeController> logger
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
            var sql = @"
                SELECT TOP (1000) 
	               [Ships].[IDNo]
                  ,[Ships].[uuid]
                  ,[Ships].[Status]
                  ,dbo.fn_ShipsStatusText([Ships].[Status]) as [StatusText]
                  ,isnull([Ships].[PaidDatetime], '') as PaidDatetime
                  ,isnull([Ships].[ShipDatetime], '') as ShipDatetime
                  ,isnull([Ships].[ReceivedDatetime], '') as ShipDatetime
                  ,[Ships].[UserId]
	              ,users.UserName
	              ,users.Addr
              FROM [Ships]
              inner join [Users] on Ships.UserId = users.UserId
              where ships.status = 0;
            ";
            var values = new { };
            var data = dbHelper.ConnDb(conn => conn.Query<ShipsInfo>(sql, values));

            return View(data);
        }
    }
}
