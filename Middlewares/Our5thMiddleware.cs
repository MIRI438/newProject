using Microsoft.AspNetCore.Http;

namespace NEWPROJECT.Middlewares;

public class Our5thMiddleware
{
    private RequestDelegate nextEfrat;

    public Our5thMiddleware(RequestDelegate nextEfrat)
    {
        this.nextEfrat = nextEfrat;
    }

    public async Task Invoke(HttpContext cntxt)
    {
        int i = 200;
        bool res1 = Helper.IsBetween(i, 1, 100);
        bool res2 = i.IsBetween(1, 100);

        await cntxt.Response.WriteAsync($"Our 5th Middleware res1: {res1}. res2: {res2}\n");
        await nextEfrat(cntxt);
        await cntxt.Response.WriteAsync("Our 5th Middleware end\n");
    }
}
public static class Our5thMiddlewareHelper
{
    public static void UseOur5th(
        this IApplicationBuilder a)
    {
        a.UseMiddleware<Our5thMiddleware>();
    }
}

public static class Helper
{
    public static bool IsBetween(
        this int number, int min, int max)
    {
        return number > min
            && number < max;
    }
}

