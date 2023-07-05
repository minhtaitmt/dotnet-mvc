using System.Text.Json;

namespace GenericRepositoryAndUnitofWork.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorMessage = ex.Message;
            Console.WriteLine($"Lỗi: {errorMessage}");

            var response = new { message = "Oops! Da co loi xay ra trong qua trinh xu ly!", error = errorMessage };
            var jsonResponse = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(jsonResponse);
        }


    }
}
