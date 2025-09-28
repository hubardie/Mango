using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Newtonsoft.Json.Linq;

namespace Mango.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccesor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccesor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccesor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);

        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _contextAccesor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccesor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        }
    }
}
