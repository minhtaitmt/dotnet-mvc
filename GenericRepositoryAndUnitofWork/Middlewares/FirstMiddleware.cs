namespace GenericRepositoryAndUnitofWork.Middlewares
{
    public class FirstMiddleware
    {
        private readonly RequestDelegate _next;
        public FirstMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items.Add("DataFirstMiddleware", $"First Middleware: URL: {context.Request.Path}");
            await _next(context);
        }
    }
}
