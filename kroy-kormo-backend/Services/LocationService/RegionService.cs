using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Models.Locations;

namespace Sob_Need_Server.Services.LocationService;

public class RegionService(AppDbContext context, IMapper mapper) : IRegionServices
{
    private readonly AppDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseDto<RegionDto>> CreateAsync(RegionDto entity)
    {
        try
        {
            var isExistRegion = await _context.Regions.AnyAsync(c => c.RegionName == entity.RegionName);
            if (isExistRegion) return new ResponseDto<RegionDto>(false, "Region Already Exist", null!);

            var regionEntity = _mapper.Map<Region>(entity);
            await _context.Regions.AddAsync(regionEntity);
            await _context.SaveChangesAsync();

            // Map Dto
            var RegionDto = _mapper.Map<RegionDto>(entity);
            return new ResponseDto<RegionDto>(true, "Region Created Successful", RegionDto);
        }
        catch (Exception ex)
        {
            return new ResponseDto<RegionDto>(false, $"{ex.Message}", null!);
        }
    }

    public Task<ResponseDto<RegionDto>> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto<IEnumerable<RegionDto>>> GetAllAsync(RegionFilterDto Filter)
    {
        try
        {

            var query = _context.Regions.AsQueryable();


            if (!string.IsNullOrEmpty(Filter.SearchText))
            {
                query = query.Where(x => x. RegionName.Contains(Filter.SearchText));
            }
            if (Filter.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == Filter.CityId);
            }


            if (Filter.OrderByAsc.HasValue)
            {
                query = Filter.OrderByAsc.Value
                    ? query.OrderBy(x => x.CreatedAt) // Ascending order
                    : query.OrderByDescending(x => x.CreatedAt); // Descending order
            }

            if (Filter.Page.HasValue && Filter.PageSize.HasValue)
            {
                query = query.Skip((Filter.Page.Value - 1) * Filter.PageSize.Value)
                             .Take(Filter.PageSize.Value);
            }

            var countries = await query
                .Select(x => _mapper.Map<RegionDto>(x))
                .ToListAsync();

            var totalCount = await query.CountAsync();

            var response = new ResponseDto<IEnumerable<RegionDto>>(true,"Regions fetched successfully", countries)
            {
                TotalCount = totalCount
            };

            return response;
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<RegionDto>>(false, $"Error: {ex.Message}", null!);
        }
    }

    public Task<ResponseDto<RegionDto>> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto<RegionDto>> UpdateAsync(Guid id, RegionDto entity)
    {
        throw new NotImplementedException();
    }
}
