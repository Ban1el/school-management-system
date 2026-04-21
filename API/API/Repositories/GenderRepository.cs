using System;
using API.Data;
using API.DTOs.Gender;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class GenderRepository(AppDbContext _context) : IGenderRepository
{
    public async Task<List<GenderDto>> GetGendersActiveAsync()
    {
        return await _context.Genders.Where(g => g.IsActive == true).Select(g => new GenderDto
        {
            Id = g.Id,
            Name = g.Name,
            DateCreated = g.DateCreated,
            CreatedBy = g.CreatedBy,
            DateModified = g.DateModified,
            ModifiedBy = g.ModifiedBy,
            IsActive = g.IsActive
        }).ToListAsync();
    }
}
