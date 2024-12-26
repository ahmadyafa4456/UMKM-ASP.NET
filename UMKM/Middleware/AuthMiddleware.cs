namespace UMKM.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true &&
                context.Request.Path.Value?.Equals("/Auth/Login", StringComparison.OrdinalIgnoreCase) == true)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }
            await _next(context);
        }
    }
}