using BePrácticasLaborales.DataAcces;

namespace BePrácticasLaborales.Services;


public abstract class CustomServiceBase
{
    protected readonly EntityDbContext _context;

    protected CustomServiceBase(EntityDbContext context)
    {
        this._context = context;
    }
}