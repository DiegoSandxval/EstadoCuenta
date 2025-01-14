namespace EstadoCuentaAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = ex.Message,
                    StackTrace = ex.StackTrace, // Opcional: útil solo para desarrollo
                    Timestamp = DateTime.UtcNow
                });

            }
        }
    }

}
