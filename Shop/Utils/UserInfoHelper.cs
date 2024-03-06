using Microsoft.AspNetCore.Http;
using Shop.Models;

namespace Shop.Utils
{
    public class UserInfoHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserId
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.GetString("UserId");
            }
            private set
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserId", value);
            }
        }
        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.GetString("UserName");
            }
            private set
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserName", value);
            }
        }
        public string Addr
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.GetString("Addr");
            }
            private set
            {
                _httpContextAccessor.HttpContext.Session.SetString("Addr", value);
            }
        }

        public User User 
        {
            get
            {
                return new User()
                {
                    UserId = UserId,
                    UserName = UserName,
                    Addr = Addr
                };
            }
            set
            {
                UserId = value.UserId; 
                UserName =value.UserName; 
                Addr = value.Addr;
            }
        }

        public bool IsLogin()
        {
            return !string.IsNullOrWhiteSpace(UserId);
        }

        internal void logOut()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
