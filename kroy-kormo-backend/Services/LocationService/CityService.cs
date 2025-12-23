using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Models.Locations;

namespace Sob_Need_Server.Services.LocationService
{
    public class CityService(AppDbContext context, IMapper mapper) : ICityServices
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseDto<CityDto>> CreateAsync(CityDto entity)
        {
            try
            {
                var isExistCity = await _context.Cities.AnyAsync(c => c.CityName == entity.CityName);
                if (isExistCity) return new ResponseDto<CityDto>(false, "City Already Exist", null!);

                var CityEntity = _mapper.Map<City>(entity);
                await _context.Cities.AddAsync(CityEntity);
                await _context.SaveChangesAsync();

                // Map Dto
                var CityDto = _mapper.Map<CityDto>(entity);
                return new ResponseDto<CityDto>(true, "City Created Successful", CityDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CityDto>(false, $"{ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CityDto>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<CityDto>>> GetAllAsync(CityFilterDto Filter)
        {
            try
            {

                var query = _context.Cities.AsQueryable();


                if (!string.IsNullOrEmpty(Filter.SearchText))
                {
                    query = query.Where(x => x. CityName.Contains(Filter.SearchText));
                }
                if (Filter.StateId.HasValue)
                {
                    //query = query.Where(x => x.StateId == Filter.StateId);
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
                    .Select(x => _mapper.Map<CityDto>(x))
                    .ToListAsync();

                var totalCount = await query.CountAsync();

                var response = new ResponseDto<IEnumerable<CityDto>>(true,"Citys fetched successfully", countries)
                {
                    TotalCount = totalCount
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<CityDto>>(false, $"Error: {ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CityDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<CityDto>> UpdateAsync(Guid id, CityDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
