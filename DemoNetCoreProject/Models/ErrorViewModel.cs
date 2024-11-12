namespace DemoNetCoreProject.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
      
    }

    public class CustomeErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomeErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception (optional)
            Console.WriteLine($"Exception: {exception.Message}");

            // Set the response status code and content type
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";

            // Return an error message or custom error page
            //    return context.Response.WriteAsync($@"
            //<html>
            //<body>
            //    <h1>An error occurred</h1>
            //    <p>{exception.Message}</p>
            //</body>
            //</html>");
            return context.Response.SendFileAsync("Views/Shared/CustomErrorPage.cshtml");
        }

    }

    }

