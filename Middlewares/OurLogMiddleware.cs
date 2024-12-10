using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace NEWPROJECT.Middlewares;

public class OurLogMiddleware
{
    private RequestDelegate next;

    public OurLogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        await c.Response.WriteAsync($"Our Log Middleware start\n");
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        await c.Response.WriteAsync("Our Log Middleware end\n");
    }
}
public static class OurLogMiddlewareHelper
{
    public static void UseOurLog(
        this IApplicationBuilder a)
    {
        a.UseMiddleware<OurLogMiddleware>();
    }
}
