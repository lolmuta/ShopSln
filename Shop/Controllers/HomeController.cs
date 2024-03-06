using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Shop.Filter;
using Shop.Models;
using Shop.Utils;
using System.Data;
using System.Diagnostics;

namespace Shop.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbHelper dbHelper;
        private readonly UserInfoHelper userInfoHelper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeController(
            ILogger<HomeController> logger
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
        public IActionResult IndexQuery()
        {
            var jsonData = dbHelper.ConnDb(conn =>
            {
                return conn.Query<ItemInfo>(
                    @"
                    SELECT [IDNo]
                          ,[name]
                          ,[count]
                          ,[src]
                          , price  
                    FROM [Items]
                    ");
            });

            return Json(jsonData);
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

        public IActionResult Item(string id)
        {
            var request = httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var ItemInfo = dbHelper.ConnDb(conn =>
            {
                return conn.QueryFirst<ItemInfo>(
                    @"
                     SELECT [IDNo]
                          ,[name]
                          ,[count]
                          , @baseUrl + '/' + [src] as src
                          , price
                      FROM [Items]
                      where IDNo = @IDNo
                    ",
                    new { IDNo = id, baseUrl });
            });
            return View(ItemInfo);
        }
        [HttpPost]
        public IActionResult UpdateBuyItem([FromBody] BuyInfo buyInfo)
        {
            try
            {
                string 檢查購買資訊結果 = 檢查購買資訊是否正確(buyInfo);
                if (!string.IsNullOrWhiteSpace(檢查購買資訊結果))
                {
                    return Ok(new { success = false, message = 檢查購買資訊結果 });
                }
                var sql = @" 
                    MERGE [dbo].[BuyItems] AS tgt
                    USING (SELECT @BuyCount as BuyCount, @Items_IDNo as Items_IDNo, @UserId as UserId) AS src
                        ON (tgt.Items_IDNo = src.Items_IDNo and tgt.UserID = src.UserId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET tgt.buyCount = src.buyCount
				
                    WHEN NOT MATCHED BY Target 
                        THEN
                            INSERT (buyCount, Items_IDNo, UserId)
                            VALUES (src.buyCount, src.Items_IDNo, src.UserId);
                ";
                var UserId = userInfoHelper.UserId;
                var paramAct = new
                {
                    BuyCount = buyInfo.buyCount,
                    Items_IDNo = buyInfo.Items_IDNo,
                    UserId = UserId
                };
                var result = dbHelper.ConnDb<int>(conn => conn.Execute(sql, paramAct));
                if (result == 1)
                {
                    // Return success message
                    return Ok(new { success = true, message = "Item updated successfully" });
                }
                else
                {
                    // Return error message
                    return BadRequest(new { success = false, message = "Failed to update item" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "測試");
                return BadRequest(new { success = false, message = "exception " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult UpdateBuyItemssss([FromBody] List<BuyInfo> buyInfosss)
        {
            try
            {
                var 檢查購買資訊是否有誤 = buyInfosss.Select(檢查購買資訊是否正確)
                                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                                .ToList();
                if (檢查購買資訊是否有誤.Count > 0)
                {
                    var joinMessage = string.Join(@"\n", 檢查購買資訊是否有誤);

                    return Ok(new { success = false, message = joinMessage });
                }
                var sql = @" 
                    MERGE [dbo].[BuyItems] AS tgt
                    USING (SELECT @BuyCount as BuyCount, @Items_IDNo as Items_IDNo, @UserId as UserId) AS src
                        ON (tgt.Items_IDNo = src.Items_IDNo and tgt.UserID = src.UserId)
                    WHEN MATCHED
                        THEN
                            UPDATE
                            SET tgt.buyCount = src.buyCount
				
                    WHEN NOT MATCHED BY Target 
                        THEN
                            INSERT (buyCount, Items_IDNo, UserId)
                            VALUES (src.buyCount, src.Items_IDNo, src.UserId);
                ";
                //var paramAct = new
                //{
                //    BuyCount = buyInfosss.buyCount,
                //    Items_IDNo = buyInfosss.IDNo,
                //    UserId = UserId
                //};
                var UserId = userInfoHelper.UserId;
                var paramAct = buyInfosss.Select(buyInfo => new
                {
                    BuyCount = buyInfo.buyCount,
                    Items_IDNo = buyInfo.Items_IDNo,
                    UserId = UserId
                });
                var result = dbHelper.ConnDb<int>(conn => conn.Execute(sql, paramAct));
                if (result == buyInfosss.Count)
                {
                    // Return success message
                    return Ok(new { success = true, message = "Item updated successfully" });
                }
                else
                {
                    // Return error message
                    return BadRequest(new { success = false, message = "Failed to update item" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "測試");
                return BadRequest(new { success = false, message = "exception " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult DeleteBuyItem([FromBody] BuyInfoDelete buyInfoDelete)
        {
            try
            {
                var sql = @" 
                    delete [BuyItems] where IDNo=@idNo;
                ";
                var UserId = userInfoHelper.UserId;
                var paramAct = new
                {
                    idNo = buyInfoDelete.IDNo
                };
                var result = dbHelper.ConnDb(conn => conn.Execute(sql, paramAct));
                if (result == 1)
                {
                    // Return success message
                    return Ok(new { success = true, message = "Item delete successfully" });
                }
                else
                {
                    // Return error message
                    return BadRequest(new { success = false, message = "Failed to delete item" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "測試");
                return BadRequest(new { success = false, message = "exception " + ex.Message });
            }
        }
        public IActionResult BuyItemList()
        {
            var request = httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var sql = @"
            SELECT 
                    BuyItems.IDNo
                  ,[BuyItems].[BuyCount]
                  ,[BuyItems].[Items_IDNo]
	              ,[Items].Name
	              ,[Items].price
                  ,@baseUrl + '/' + [Items].src as src
            FROM 
                [BuyItems]
            left join [dbo].[Items] on [Items].IDNo = [BuyItems].[Items_IDNo]
            where 1 = 1
	            and [BuyItems].UserId = @UserId
            ";
            var UserId = userInfoHelper.UserId;
            var list = dbHelper.ConnDb(conn =>
                 conn.Query<BuyItem>(sql, new { UserId, baseUrl })
            );

            return View(list);
        }
        public IActionResult ReadyPay()
        {
            var request = httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var sql = @"
            SELECT [BuyItems].[IDNo]
                  ,[BuyItems].[BuyCount]
                  ,[BuyItems].[Items_IDNo]
	              ,[Items].Name
	              ,[Items].price
                  ,@baseUrl + '/' + [Items].src as src
            FROM 
                [BuyItems]
            left join [dbo].[Items] on [Items].IDNo = [BuyItems].[Items_IDNo]
            where 1 = 1
	            and [BuyItems].UserId = @UserId
            ";
            var UserId = userInfoHelper.UserId;
            var list = dbHelper.ConnDb(conn =>
                 conn.Query<BuyItem>(sql, new { UserId, baseUrl })
            );

            return View(list);  
        }
        private string 檢查購買資訊是否正確(BuyInfo buyInfo)
        {
            var sql確認物品存在 = @"select 1 from Items where IDNo = @IDNo;";
            var param確認物品存在 = new { IDNo = buyInfo.Items_IDNo };
            var 確認物品存在 = dbHelper.ConnDb(conn =>
                conn.ExecuteScalar<string>(sql確認物品存在, param確認物品存在)) == "1";
            if (!確認物品存在)
            {
                return "該項物品不存在";
            }
            var sql確認物品庫存量足夠 = @"select 1 from Items where IDNo = @IDNo and count >=@buyCount;";
            var param確認物品庫存量足夠 = new
            {
                BuyCount = buyInfo.buyCount,
                IDNo = buyInfo.Items_IDNo
            };
            var 確認物品庫存量足夠 = dbHelper.ConnDb(conn => conn.ExecuteScalar<string>(sql確認物品庫存量足夠, param確認物品庫存量足夠)) == "1";
            if (!確認物品庫存量足夠)
            {
                return "認物品庫存不足";
            }
            return "";
        }
        [HttpPost]
        public IActionResult usp_ReadyPay()
        {
            try
            {
                var storedProcedureName = "usp_ReadyPay";
                var UserId = userInfoHelper.UserId;
                var values = new { UserId };

                var 預存執行是否有誤 = dbHelper.ConnDb(conn => conn.QueryFirst<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure));
                if (!string.IsNullOrWhiteSpace(預存執行是否有誤))
                {
                    return Ok(new { success = false, message = 預存執行是否有誤 });
                }

                return Ok(new { success = true, message = "" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "測試");
                return BadRequest(new { success = false, message = "exception " + ex.Message });
            }
        }
    }
    public class BuyInfo
    {
        public int Items_IDNo { get; set; }
        public int buyCount { get; set; }
    }
    public class BuyInfoDelete
    {
        public int IDNo { get; set; }
    }
}