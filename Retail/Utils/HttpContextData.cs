namespace Retail.Utils
{
    public static class HttpContextData
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor?.HttpContext;

        public static string GetUser()
        {
            return Current?.User?.Identity?.Name ?? "Anonymous";
        } 
    }
}
