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
    public class StateService(AppDbContext context, IMapper mapper) : IStateServices
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseDto<StateDto>> CreateAsync(StateDto entity)
        {
            try
            {
                var isExistState = await _context.States.AnyAsync(c => c.StateName == entity.StateName);
                if (isExistState) return new ResponseDto<StateDto>(false, "State Already Exist", null!);

                var StateEntity = _mapper.Map<State>(entity);
                await _context.States.AddAsync(StateEntity);
                await _context.SaveChangesAsync();

                // Map Dto
                var StateDto = _mapper.Map<StateDto>(entity);
                return new ResponseDto<StateDto>(true, "State Created Successful", StateDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<StateDto>(false, $"{ex.Message}", null!);
            }
        }

        public Task<ResponseDto<StateDto>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<StateDto>>> GetAllAsync(StateFilterDto Filter)
        {
            try
            {

                var query = _context.States.AsQueryable();


                if (!string.IsNullOrEmpty(Filter.SearchText))
                {
                    query = query.Where(x => x. StateName.Contains(Filter.SearchText));
                }
                if (Filter.CountryId.HasValue)
                {
                    query = query.Where(x => x.CountryId == Filter.CountryId);
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
                    .Select(x => _mapper.Map<StateDto>(x))
                    .ToListAsync();

                var totalCount = await query.CountAsync();

                var response = new ResponseDto<IEnumerable<StateDto>>(true,"States fetched successfully", countries)
                {
                    TotalCount = totalCount
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<StateDto>>(false, $"Error: {ex.Message}", null!);
            }
        }

        public Task<ResponseDto<StateDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<StateDto>> UpdateAsync(Guid id, StateDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
