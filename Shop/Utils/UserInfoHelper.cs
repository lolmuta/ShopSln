namespace Shop.Utils
{
    public class UserInfoHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            //var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            //return userId;
            return "1";
        }
        public void SetUserId(string UserId)
        {
            _httpContextAccessor.HttpContext.Session.SetString("UserId", UserId);
        }
        public string UserName { get; set; }
        public bool IsLogin()
        {
            //todo session 要能用
            return true;
            //return !string.IsNullOrWhiteSpace(GetUserId());
        }
    }
}
