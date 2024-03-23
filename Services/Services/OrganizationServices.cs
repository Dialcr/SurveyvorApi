using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Services.Dtos;

namespace Services.Services;

public class OrganizationServices : CustomServiceBase
{
    public OrganizationServices(EntityDbContext context) : base(context)
    {
        
    }
    //todo editar las organizaciones
    // TODO: editar solo un usuario admin

    public async Task<OneOf<ResponseErrorDto, University>> EditOrganization(int organizationId, UniversityIntupDto universityInputDto)
    {
        var organization = await _context.University.SingleOrDefaultAsync(x => x.Id == organizationId);
        
        if (organization is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Ministry not found"
            };
        }

        if (!universityInputDto.Name.IsNullOrEmpty())
        {
            organization.Name = universityInputDto.Name;
        }
        organization.Enable = universityInputDto.Enable;
        
        await _context.SaveChangesAsync();
        return organization;

    }

    public async Task<OneOf<ResponseErrorDto, UniversityOutputDto>> Adduniversity(UniversitiIntputDto universitiIntputDto)
    {
        var request = await _context.University.SingleOrDefaultAsync(x=>x.Name == universitiIntputDto.Name);
        if (request is not null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 400,
                ErrorMessage = "University already exists"
            };
        }

        var newUniversity = new University()
        {
            Name = universitiIntputDto.Name,
            Enable = universitiIntputDto.Enable,
        };
        _context.University.Add(newUniversity);
        await _context.SaveChangesAsync();
        var university = await 
            _context.University.FirstOrDefaultAsync(x =>
                x.Name == universitiIntputDto.Name);
        return university.ToUniversityOutputDto();
    }

    public async Task<OneOf<ResponseErrorDto, University>> GetUniversity(int universityId)
    {
        var university = await _context.University.SingleOrDefaultAsync(x=>x.Id == universityId);
        if (university is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "University not found"
            };
        }

        return university;
    }
   
    public IEnumerable<UniversityOutputDto> GetAllUniversity()
    {
        return _context.University.Where(x => x.Enable).Select(x=>x.ToUniversityOutputDto());
    }
    

    public OneOf<ResponseErrorDto, ICollection<University>> UniersitiesWithMoreSurvey()
    {
        var organizations = _context.University.Include(x=>x.Surveys)
            .OrderByDescending(x => x.Surveys.Count);
        if (!organizations.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }

        var cant =organizations.First().Surveys.Count();
        var organizationsMax = organizations.Where(x=>x.Surveys.Count() == cant).ToList();
        
        return organizationsMax;
    }
    public OneOf<ResponseErrorDto, ICollection<University>> OrganizatinoWithFewerSurvey()
    {
        var organizations = _context.University.Include(x=>x.Surveys)
            .OrderBy(x => x.Surveys.Count);
        if (!organizations.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }

        var cant =organizations.First().Surveys.Count();
        var organizationsMax = organizations.Where(x=>x.Surveys.Count() == cant).ToList();
        
        return organizationsMax;
    }
    public  IEnumerable<UniversityOutputDto> AllUniversity()
    {
        var universities = _context.University.Select(x=>x.ToUniversityOutputDto());
        return universities;
    }
    
    
}