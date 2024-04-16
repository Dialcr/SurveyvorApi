using BePrácticasLaborales.DataAcces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BePrácticasLaborales.Middleware;

public class CheckSurveyDateAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var dbContext = (EntityDbContext)
            context.HttpContext.RequestServices.GetService(typeof(EntityDbContext))!;
        dbContext
            .Surveys.Where(x => x.EndDate.Date < DateTime.Now.Date)
            .ToList()
            .ForEach(x => x.Available = false);
        await dbContext.SaveChangesAsync();

        await next();
    }
}
