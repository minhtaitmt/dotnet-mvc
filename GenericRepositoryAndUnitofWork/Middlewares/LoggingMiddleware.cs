namespace GenericRepositoryAndUnitofWork.Middlewares
{
    public class LoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //if (context.Request.Path.StartsWithSegments("/api/Users"))
            //{
            //    LogRequest(context.Request);

            //    await next(context);
            //}
            LogRequest(context.Request);
            await next(context);
        }

        private void LogRequest(HttpRequest request)
        {
            Console.WriteLine($"Request: {request.Method} {request.Path}");
            Console.WriteLine("Headers:");
            foreach (var header in request.Headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }
            Console.WriteLine();
        }
    }
}
