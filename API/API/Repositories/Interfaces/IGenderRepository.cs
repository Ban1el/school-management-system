using System;
using API.DTOs.Gender;

namespace API.Repositories.Interfaces;

public interface IGenderRepository
{
    Task<List<GenderDto>> GetGendersActiveAsync();
}
