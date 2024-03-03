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

    public async Task<OneOf<ResponseErrorDto, Organization>> EditOrganization(int organizationId, OrganizatinosIntupDto organizationInputDto)
    {
        var organization = await _context.Organization.SingleOrDefaultAsync(x => x.Id == organizationId);
        
        if (organization is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Ministry not found"
            };
        }

        if (!organizationInputDto.Name.IsNullOrEmpty())
        {
            organization.Name = organizationInputDto.Name;
        }
        organization.Enable = organizationInputDto.Enable;
        
        await _context.SaveChangesAsync();
        return organization;

    }

    public async Task<OneOf<ResponseErrorDto, University>> Adduniversity(UniversitiIntputDto universitiIntputDto)
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
        var ministery = await _context.Ministery.SingleOrDefaultAsync(x=>x.Id == universitiIntputDto.MinisteryId);
        if (ministery is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Ministry not found"
            };
        }

        var newUniversity = new University()
        {
            Name = universitiIntputDto.Name,
            Enable = universitiIntputDto.Enable,
        };
        _context.University.Add(newUniversity);
        await _context.SaveChangesAsync();
        
        return newUniversity;
    }
    public async Task<OneOf<ResponseErrorDto, Ministery>> AddMinnistery(UniversitiIntputDto newMinistery)
    {
        var request = await _context.Ministery.SingleOrDefaultAsync(x=>x.Name == newMinistery.Name);
        if (request is not null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 400,
                ErrorMessage = "ministery already exists"
            };
        }
        var ministery = new Ministery()
        {
            Name = newMinistery.Name,
            Enable = newMinistery.Enable,
        };
        _context.Ministery.Add(ministery);
        await _context.SaveChangesAsync();
        
        return ministery;
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
    public OneOf<ResponseErrorDto, ICollection<University>> GetAllUniversity(int ministeryid)
    {
        var university =  _context.University.Where(x=>x.MinisteryId == ministeryid);
        if (!university.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "University list not found"
            };
        }

        return university.ToList();
    }
    public async Task<OneOf<ResponseErrorDto, Ministery>> GetMinistery(int ministryId)
    {
        var ministery = await _context.Ministery.SingleOrDefaultAsync(x=>x.Id == ministryId);
        if (ministery is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Ministery not found"
            };
        }

        return ministery;
    }

    public OneOf<ResponseErrorDto, ICollection<Organization>> OrganizatinoWithMoreSurvey()
    {
        var organizations = _context.Organization.Include(x=>x.Surveys)
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
    public OneOf<ResponseErrorDto, ICollection<Organization>> OrganizatinoWithFewerSurvey()
    {
        var organizations = _context.Organization.Include(x=>x.Surveys)
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
    public OneOf<ResponseErrorDto, ICollection<Organization>> AllOrganizatino()
    {
        var organizations = _context.Organization.ToList();
        if (!organizations.Any())
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "Organization list not found"
            };
        }
        return organizations;
    }
    
    
}