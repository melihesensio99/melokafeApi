using Serilog;
using System.Diagnostics;

namespace kafeApi.API.Middlewares
{
   
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var request = context.Request; 
            var ip = context.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
            Log.Information("Gelen istek: {Method} {Path} - {IP}", request.Method, request.Path, ip);

            context.Response.OnStarting(() =>
            {
                sw.Stop();
                context.Response.Headers.Append("X-Process-Time", $"{sw.ElapsedMilliseconds}ms");
                return Task.CompletedTask;
            });

            try
            {
                await _next(context);
                sw.Stop();

                Log.Information("Yanıt : {StatusCode} - Sure : {Elapsed} ms",
                    context.Response.StatusCode, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log.Error(ex, "Hata oluştu. Sure: {Elapsed} ms", sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
