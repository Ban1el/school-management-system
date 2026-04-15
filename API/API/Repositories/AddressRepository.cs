using System;
using API.Data;
using API.DTOs.Address;
using API.DTOs.Dropdown;
using API.Helpers.Pagination;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class AddressRepository(AppDbContext _context) : IAddressRepository
{
    public async Task<List<RegionDto>> GetRegionsAsync()
    {
        return await _context.Regions.Select(r => new RegionDto
        {
            Id = r.Id,
            Name = r.Name,
            DateCreated = r.DateCreated,
            CreatedBy = r.CreatedBy,
            DateModified = r.DateModified,
            ModifiedBy = r.ModifiedBy,
            IsActive = r.IsActive
        }).ToListAsync();
    }

    public async Task<List<ProvinceDto>> GetProvincesAsync(int id)
    {
        return await _context.Provinces.Where(p => p.RegionId == id).Select(p => new ProvinceDto
        {
            Id = p.Id,
            Name = p.Name,
            regionId = p.RegionId,
            DateCreated = p.DateCreated,
            CreatedBy = p.CreatedBy,
            DateModified = p.DateModified,
            ModifiedBy = p.ModifiedBy,
            IsActive = p.IsActive
        }).ToListAsync();
    }

    public async Task<List<CityMunicipalityDto>> GetCitiesMunicipalitiesAsync(int id, bool byRegion = false)
    {
        if (!byRegion)
        {
            return await _context.CitiesMunicipalities.Where(c => c.ProvinceId == id).Select(c => new CityMunicipalityDto
            {
                Id = c.Id,
                Name = c.Name,
                ProvinceId = c.Id,
                DateCreated = c.DateCreated,
                CreatedBy = c.CreatedBy,
                DateModified = c.DateModified,
                ModifiedBy = c.ModifiedBy,
                IsActive = c.IsActive
            }).ToListAsync();
        }
        else
        {
            return await _context.CitiesMunicipalities.Where(c => c.RegionId == id).Select(c => new CityMunicipalityDto
            {
                Id = c.Id,
                Name = c.Name,
                RegionId = c.RegionId,
                DateCreated = c.DateCreated,
                CreatedBy = c.CreatedBy,
                DateModified = c.DateModified,
                ModifiedBy = c.ModifiedBy,
                IsActive = c.IsActive
            }).ToListAsync();
        }
    }

    public async Task<List<BarangayDto>> GetBarangaysAsync(int id)
    {
        return await _context.Barangays.Where(b => b.CityId == id).Select(b => new BarangayDto
        {
            Id = b.Id,
            Name = b.Name,
            CityId = b.CityId,
            DateCreated = b.DateCreated,
            CreatedBy = b.CreatedBy,
            DateModified = b.DateModified,
            ModifiedBy = b.ModifiedBy,
            IsActive = b.IsActive
        }).ToListAsync();
    }

    public async Task<PaginatedResult<DropdownDto>> GetRegionsPaginatedAsync(string? search, int pageNumber, int pageSize)
    {
        var query = _context.Regions
            .Where(r => r.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Name.Contains(search));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new DropdownDto
            {
                Id = r.Id,
                Name = r.Name,
            })
            .ToListAsync();

        return PaginationHelper.Create(items, totalCount, pageNumber, pageSize);
    }


    public async Task<PaginatedResult<DropdownDto>> GetProvincesPaginatedAsync(string? search, int pageNumber, int pageSize, int regionId)
    {
        var query = _context.Provinces
              .Where(r => r.IsActive && r.RegionId == regionId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Name.Contains(search));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new DropdownDto
            {
                Id = r.Id,
                Name = r.Name,
            })
            .ToListAsync();

        return PaginationHelper.Create(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PaginatedResult<DropdownDto>> GetCitiesMunicipalitiesPaginatedAsync(string? search, int pageNumber, int pageSize, int id, bool byRegion = false)
    {
        var query = byRegion ?
                     _context.CitiesMunicipalities
                     .Where(r => r.IsActive && r.RegionId == id) :
                     _context.CitiesMunicipalities
                     .Where(r => r.IsActive && r.ProvinceId == id);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Name.Contains(search));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new DropdownDto
            {
                Id = r.Id,
                Name = r.Name,
            })
            .ToListAsync();

        return PaginationHelper.Create(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PaginatedResult<DropdownDto>> GetBarangaysPaginatedAsync(string? search, int pageNumber, int pageSize, int cityId)
    {
        var query = _context.Barangays
            .Where(r => r.IsActive && r.CityId == cityId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Name.Contains(search));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new DropdownDto
            {
                Id = r.Id,
                Name = r.Name,
            })
            .ToListAsync();

        return PaginationHelper.Create(items, totalCount, pageNumber, pageSize);
    }
}
