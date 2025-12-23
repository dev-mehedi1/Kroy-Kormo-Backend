using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.Interface.Location;
using Sob_Need_Server.Models.Locations;

namespace Sob_Need_Server.Services.LocationService
{
    public class CountryService(AppDbContext context, IMapper mapper) : ICountryService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseDto<CountryDto>> CreateAsync(CountryDto entity)
        {
            try
            {
                var isExistCountry = await _context.Countries.AnyAsync(c => c.CountryName == entity.CountryName);
                if (isExistCountry) return new ResponseDto<CountryDto>(false, "Country Already Exist", null!);

                var countryEntity = _mapper.Map<Country>(entity);
                await _context.Countries.AddAsync(countryEntity);
                await _context.SaveChangesAsync();

                // Map Dto
                var countryDto = _mapper.Map<CountryDto>(entity);
                return new ResponseDto<CountryDto>(true, "Country Created Successful", countryDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CountryDto>(false, $"{ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CountryDto>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<CountryDto>>> GetAllAsync(FilterDto Filter)
        {
            try
            {

                var query = _context.Countries.AsQueryable();


                if (!string.IsNullOrEmpty(Filter.SearchText))
                {
                    query = query.Where(x => x. CountryName.Contains(Filter.SearchText));
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
                    .Select(x => _mapper.Map<CountryDto>(x))
                    .ToListAsync();

                var totalCount = await query.CountAsync();

                var response = new ResponseDto<IEnumerable<CountryDto>>(true,"Countries  fetched successfully", countries)
                {
                    TotalCount = totalCount
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<CountryDto>>(false, $"Error: {ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CountryDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<CountryDto>> UpdateAsync(Guid id, CountryDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
