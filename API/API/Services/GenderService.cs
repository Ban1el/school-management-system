using System;
using API.Data;
using API.DTOs.Gender;
using API.Repositories.Interfaces;

namespace API.Services;

public class GenderService
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public GenderService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<List<GenderDto>> GetGendersActiveAsync()
    {
        return await _uow.Genders.GetGendersActiveAsync();
    }
}
