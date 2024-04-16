using BePrácticasLaborales.DataAcces;

namespace BePrácticasLaborales.Middleware;

public class CheckSurveyDateMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, EntityDbContext dbContext)
    {
        //var a = dbContext.Surveys.ToList();
        var b = dbContext.Surveys.Where(x => x.EndDate.Date > DateTime.Now.Date).ToList();
        b.ForEach(x => Console.WriteLine(x.EndDate));
        await dbContext.SaveChangesAsync();
        await next(context);
    }
}

public static class CheckSurveyDateMiddlewareExtensions
{
    public static IApplicationBuilder UseCheckSurveyDate(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CheckSurveyDateMiddleware>();
    }
}
