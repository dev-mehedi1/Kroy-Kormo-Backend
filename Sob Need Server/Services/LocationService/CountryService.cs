using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.Interface.Location;

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
                var isExistCountry = await _context.Countries.AnyAsync(c => c.CountryId == entity.CountryId);
                if (isExistCountry) return new ResponseDto<CountryDto>(false, "Country Already Exist", null!);

                var countryEntity = _mapper.Map<CountryDto>(entity);
                await _context.AddAsync(countryEntity);
                await _context.SaveChangesAsync();
                return new ResponseDto<CountryDto>(true, "Country Already Exist", countryEntity);
            }
            catch (Exception ex)
            {
                return new ResponseDto<CountryDto>(false, $"{ex.Message}", null!);
            }
        }

        public Task<ResponseDto<CountryDto>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsync(FilterDto Filter)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<CountryDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<CountryDto>> UpdateAsync(int id, CountryDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
