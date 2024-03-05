using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Shop.Utils;

namespace Shop.Filter
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly UserInfoHelper userInfoHelper;

        public CustomAuthorizeFilter(UserInfoHelper _userInfoHelper)
        {
            userInfoHelper = _userInfoHelper;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 检查 Session 中是否存在用户登录信息
            if (!userInfoHelper.IsLogin())
            {
                // 用户未登录，执行相应的逻辑，比如重定向到登录页面
                context.Result = new RedirectToActionResult("Login", "Index", null);
            }
        }
    }
}
