using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
namespace Mango.Services.ShoppingCartAPI.Utility
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler // Similar a Middleware de CORE pero para cliente side
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccesor)
        {
             _httpContextAccesor = httpContextAccesor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccesor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }   
    }
}
