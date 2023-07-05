using Microsoft.AspNetCore.Http;

namespace GenericRepositoryAndUnitofWork.Middlewares
{
    public class SecondMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var datafromFirstMiddleware = context.Items["DataFirstMiddleware"];
            var datafromDefault = context.Items["DataFromDefault"];
            if (context.Request.Path == "/abc")
            {
                context.Response.Headers.Add("SecondMiddleware", "Khong the truy cap");
                await context.Response.WriteAsync("SencondMiddleware: Khong the truy cap");
                if (datafromFirstMiddleware != null)
                    await context.Response.WriteAsync((string)datafromFirstMiddleware);
                if (datafromDefault != null)
                    await context.Response.WriteAsync((string)datafromDefault);

            }
            else
            {
                context.Response.Headers.Add("SecondMiddleware", "Co the truy cap");
                await context.Response.WriteAsync("Co the truy cap");
                if (datafromFirstMiddleware != null)
                    await context.Response.WriteAsync((string)datafromFirstMiddleware);
                if (datafromDefault != null)
                    await context.Response.WriteAsync((string)datafromDefault);
                await next(context);
            }

        }
    }
}
