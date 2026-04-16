using System;
using API.Models.Address;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using API.DTOs.Address;
using API.DTOs.PsgcApi;

namespace API.Data.Seed;

public class AddressSeedData
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://barangay-api.hawitsu.xyz/")
    };

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context == null) throw new ArgumentNullException("Null DBContext");

        await SeedRegionsAsync(context);
        await SeedProvincesAsync(context);
        await SeedCitiesAsync(context);
        await SeedBarangaysAsync(context);
    }

    // -------------------------------------------------------
    // REGIONS
    // -------------------------------------------------------
    private static async Task SeedRegionsAsync(AppDbContext context)
    {
        if (context.Regions.Any())
        {
            Console.WriteLine("Regions already seeded, skipping...");
            return;
        }

        Console.WriteLine("Seeding Regions...");

        var response = await _httpClient.GetAsync("regions");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<string>>(json)?.Distinct().ToList();

        if (result is null) return;

        var regions = result.Select(name => new Region
        {
            Name = name,
            DateCreated = DateTime.UtcNow,
            CreatedBy = null,
            ModifiedBy = null,
            IsActive = true
        }).ToList();

        await context.Regions.AddRangeAsync(regions);
        await context.SaveChangesAsync();

        Console.WriteLine($"Seeded regions");
    }

    // -------------------------------------------------------
    // PROVINCES
    // -------------------------------------------------------
    private static async Task SeedProvincesAsync(AppDbContext context)
    {
        if (context.Provinces.Any())
        {
            Console.WriteLine("Provinces already seeded, skipping...");
            return;
        }

        Console.WriteLine("Seeding Provinces...");

        // get all seeded regions to match by name
        var regions = await context.Regions.ToListAsync();

        if (regions == null || regions.Count == 0) return;

        foreach (var region in regions)
        {
            var response = await _httpClient.GetAsync($"{region.Name}/provinces_and_highly_urbanized_cities");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<string>>(json)?.Distinct().ToList();

            if (result is null) continue;

            if (region.Name == "National Capital Region (NCR)")
            {
                var citiesMunicipalities = result.Select(name => new CityMunicipality
                {
                    Name = name,
                    RegionId = region.Id,
                    ProvinceId = null,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = null,
                    ModifiedBy = null,
                    IsActive = true
                }).ToList();

                await context.CitiesMunicipalities.AddRangeAsync(citiesMunicipalities);
            }
            else
            {
                var provinces = result.Select(name => new Province
                {
                    Name = name,
                    RegionId = region.Id,
                    DateCreated = DateTime.Now,
                    CreatedBy = null,
                    ModifiedBy = null,
                    IsActive = true
                }).ToList();

                await context.Provinces.AddRangeAsync(provinces);
            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded provinces");
        }
    }

    // -------------------------------------------------------
    // CITIES
    // -------------------------------------------------------
    private static async Task SeedCitiesAsync(AppDbContext context)
    {
        if (context.CitiesMunicipalities.Any(c => c.RegionId != null && c.ProvinceId != null))
        {
            Console.WriteLine("Cities already seeded, skipping...");
            return;
        }

        Console.WriteLine("Seeding Cities...");

        var provinces = await context.Provinces
        .Include(p => p.Region)
        .Select(p => new
        {
            p.Id,
            p.Name,
            RegionName = p.Region.Name
        })
        .ToListAsync();

        if (provinces == null || provinces.Count == 0) return;

        foreach (var province in provinces)
        {
            var response = await _httpClient.GetAsync($"{province.RegionName}/{province.Name}/municipalities_and_cities");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<string>>(json)?.Distinct().ToList();

            if (result is null || province.RegionName == "National Capital Region (NCR)") continue;

            var citiesMunicipalities = result.Select(name => new CityMunicipality
            {
                Name = name,
                RegionId = null,
                ProvinceId = province.Id,
                DateCreated = DateTime.UtcNow,
                CreatedBy = null,
                ModifiedBy = null,
                IsActive = true
            }).ToList();

            await context.CitiesMunicipalities.AddRangeAsync(citiesMunicipalities);
        }

        await context.SaveChangesAsync();

        Console.WriteLine($"Seeded cities");
    }

    // -------------------------------------------------------
    // BARANGAYS
    // -------------------------------------------------------
    private static async Task SeedBarangaysAsync(AppDbContext context)
    {
        if (context.Barangays.Any())
        {
            Console.WriteLine("Barangays already seeded, skipping...");
            return;
        }

        Console.WriteLine("Seeding Barangays...");


        var citiesMunicipalities = await context.CitiesMunicipalities
     .Include(c => c.Province)!
         .ThenInclude(p => p!.Region)  // ← null-forgiving operator
     .Select(c => new
     {
         c.Id,
         c.Name,
         RegionName = c.Province != null ? c.Province.Region.Name : "National Capital Region (NCR)",
         ProvinceName = c.Province != null ? c.Province.Name : ""
     })
     .ToListAsync();

        if (citiesMunicipalities == null || citiesMunicipalities.Count == 0) return;

        foreach (var city in citiesMunicipalities)
        {
            var regionName = Uri.EscapeDataString(city.RegionName);
            var provinceName = Uri.EscapeDataString(city.ProvinceName);
            var cityName = Uri.EscapeDataString(city.Name);

            if (city.Name == "City of Manila")
            {
                List<string> districts = new List<string>
                {
                    "Binondo",
                    "Ermita",
                    "Intramuros",
                    "Malate",
                    "Paco",
                    "Pandacan",
                    "Port Area",
                    "Quiapo",
                    "Sampaloc",
                    "San Miguel",
                    "San Nicolas",
                    "Santa Ana",
                    "Santa Cruz",
                    "Tondo I"
                };

                foreach (var district in districts)
                {
                    var response = await _httpClient.GetAsync($"{regionName}/{cityName}/{district}/barangays");

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"REGION: {regionName}");
                        Console.WriteLine($"PROVINCE: {provinceName}");
                        Console.WriteLine($"CITY: {cityName}");
                        Console.WriteLine($"Skipping {city.Name} — {response.StatusCode}");
                        continue;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<string>>(json)?.Distinct().ToList();

                    if (result is null) continue;

                    var barangays = result.Select(name => new Barangay
                    {
                        Name = name,
                        CityId = city.Id,
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = null,
                        ModifiedBy = null,
                        IsActive = true
                    }).ToList();

                    await context.Barangays.AddRangeAsync(barangays);
                }
            }
            else
            {
                var response = city.RegionName == "National Capital Region (NCR)"
               ? await _httpClient.GetAsync($"{regionName}/{cityName}/{cityName}/barangays")
               : await _httpClient.GetAsync($"{regionName}/{provinceName}/{cityName}/barangays");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"REGION: {regionName}");
                    Console.WriteLine($"PROVINCE: {provinceName}");
                    Console.WriteLine($"CITY: {cityName}");
                    Console.WriteLine($"Skipping {city.Name} — {response.StatusCode}");
                    continue;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<string>>(json)?.Distinct().ToList();

                if (result is null) continue;

                var barangays = result.Select(name => new Barangay
                {
                    Name = name,
                    CityId = city.Id,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = null,
                    ModifiedBy = null,
                    IsActive = true
                }).ToList();

                await context.Barangays.AddRangeAsync(barangays);
            }

            await context.SaveChangesAsync();
        }


        Console.WriteLine($"Seeded barangays");
    }
}