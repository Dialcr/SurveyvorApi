using BePrácticasLaborales.DataAcces;
using DataAcces.Entities;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Services.Dtos;

namespace Services.Services;

public class OrganizationServices(EntityDbContext context) : CustomServiceBase(context)
{
    public async Task<OneOf<ResponseErrorDto, University>> EditOrganization(int organizationId, UniversityIntupDto universityInputDto)
    {
        var organization = await _context.University.SingleOrDefaultAsync(x => x.Id == organizationId);
        
        if (organization is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "University not found"
            };
        }

        if (!universityInputDto.Name.IsNullOrEmpty())
        {
            organization.Name = universityInputDto.Name;
            organization.Email = universityInputDto.Email;
            organization.Description = universityInputDto.Description;
            organization.FacultiesNumber = universityInputDto.FacultiesNumber <= 0 ? 1 : universityInputDto.FacultiesNumber;
            organization.BgImage = universityInputDto.BgImage ??
                                   organization.BgImage;
            organization.ProfileImage = universityInputDto.ProfileImage ??
                                        organization.ProfileImage;
        }
        organization.Enable = universityInputDto.Enable;
        
        await _context.SaveChangesAsync();
        return organization;

    }

    public async Task<OneOf<ResponseErrorDto, UniversityOutputDto>> AddUniversityAsync(UniversityIntupDto universitiIntputDto)
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
            Email = universitiIntputDto.Email,
            Description = universitiIntputDto.Description,
            FacultiesNumber = universitiIntputDto.FacultiesNumber,
            BgImage = universitiIntputDto.BgImage ?? await File.ReadAllBytesAsync("./../DataAcces/Images/university2.jpg"),
            ProfileImage = universitiIntputDto.ProfileImage ?? await File.ReadAllBytesAsync("./../DataAcces/Images/university1.jpg")
        };
        _context.University.Add(newUniversity);
        await _context.SaveChangesAsync();
        var university = await 
            _context.University.FirstOrDefaultAsync(x =>
                x.Name == universitiIntputDto.Name);
        return university.ToUniversityOutputDto();
    }

    public async Task<OneOf<ResponseErrorDto, UniversityOutputDto>> GetUniversity(int universityId)
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

        return university.ToUniversityOutputDto();
    }
    public async Task<OneOf<ResponseErrorDto, UniversityOutputDto>> GetUniversityByUserAsync(int userId)
    {
        var university = await _context.University
            .SingleOrDefaultAsync(x=>x.Users!.Any(y=>y.Id==userId) );
        if (university is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "University not found"
            };
        }

        return university.ToUniversityOutputDto();
    }
   
    public IEnumerable<UniversityOutputDto> GetAllUniversity()
    {
        return _context.University.Where(x => x.Enable).Select(x=>x.ToUniversityOutputDto());
    }
    public OneOf<ResponseErrorDto, UniversityOutputDto> DisableUniversity(int universityId)
    {
        var university = _context.University.SingleOrDefault(x => x.Id == universityId);
        if (university is null)
        {
            return new ResponseErrorDto()
            {
                ErrorCode = 404,
                ErrorMessage = "University not found"
            };
        }
        university.Enable = false;
        _context.SaveChanges();
        return university.ToUniversityOutputDto();
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