using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.Interface;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Services.ConditionService
{
    public class ConditionService(AppDbContext context, IMapper mapper) : IConditionService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<ConditionDto>> CreateAsync(ConditionDto entity)
        {
            try
            {
                bool exists = await _context.Conditions.AnyAsync(x => x.ConditionName == entity.ConditionName);
                if (exists)
                {
                    return new ResponseDto<ConditionDto>(false, "Condition name already exists.", null!);
                }

                string slug = StringUtilities.GenerateSlug(entity.ConditionName);

                bool slugExists = await _context.Conditions.AnyAsync(x => x.Slug == slug);
                if (slugExists)
                {
                    return new ResponseDto<ConditionDto>(false, "A condition with the same slug already exists.", null!);
                }

                var conditionEntity = _mapper.Map<Condition>(entity);
                conditionEntity.Slug = slug;

                await _context.Conditions.AddAsync(conditionEntity);
                await _context.SaveChangesAsync();

                var conditionDto = _mapper.Map<ConditionDto>(conditionEntity);

                return new ResponseDto<ConditionDto>(true, "Condition created successfully.", conditionDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ConditionDto>(false, $"An error occurred: {ex.Message}", null!);
            }
        }

        public async Task<ResponseDto<ConditionDto>> DeleteAsync(Guid id)
        {
            try
            {
                var condition = await _context.Conditions.FindAsync(id);
                if (condition == null)
                {
                    return new ResponseDto<ConditionDto>(false, "Condition not found.", null!);
                }

                _context.Conditions.Remove(condition);
                await _context.SaveChangesAsync();

                return new ResponseDto<ConditionDto>(true, "Condition deleted successfully.", null!);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ConditionDto>(false, $"An error occurred: {ex.Message}", null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<ConditionDto>>> GetAllAsync(ConditionFilterDto filter)
        {
            try
            {
                var query = _context.Conditions.AsQueryable();

                if (!string.IsNullOrEmpty(filter.SearchText))
                {
                    query = query.Where(x => x.ConditionName.Contains(filter.SearchText));
                }

                if (filter.OrderByAsc.HasValue)
                {
                    query = filter.OrderByAsc.Value
                        ? query.OrderBy(x => x.ConditionName)
                        : query.OrderByDescending(x => x.ConditionName);
                }
                else
                {
                    query = query.OrderBy(x => x.ConditionName);
                }

                var totalCount = await query.CountAsync();

                if (filter.Page.HasValue && filter.PageSize.HasValue)
                {
                    query = query.Skip((filter.Page.Value - 1) * filter.PageSize.Value)
                                 .Take(filter.PageSize.Value);
                }

                var conditions = await query
                   .ProjectTo<ConditionDto>(_mapper.ConfigurationProvider)
                   .ToListAsync();

                var response = new ResponseDto<IEnumerable<ConditionDto>>(
                    true,
                    "Conditions fetched successfully",
                    conditions)
                {
                    TotalCount = totalCount
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<ConditionDto>>(false, $"Error: {ex.Message}", null!);
            }
        }

        public async Task<ResponseDto<ConditionDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var condition = await _context.Conditions
                    .ProjectTo<ConditionDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.ConditionId == id);

                if (condition == null)
                {
                    return new ResponseDto<ConditionDto>(false, "Condition not found.", null!);
                }

                return new ResponseDto<ConditionDto>(true, "Condition fetched successfully.", condition);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ConditionDto>(false, $"An error occurred: {ex.Message}", null!);
            }
        }

        public async Task<ResponseDto<ConditionDto>> UpdateAsync(Guid id, ConditionDto entity)
        {
            try
            {
                var conditionEntity = await _context.Conditions.FindAsync(id);
                if (conditionEntity == null)
                {
                    return new ResponseDto<ConditionDto>(false, "Condition not found.", null!);
                }

                bool exists = await _context.Conditions.AnyAsync(x => x.ConditionName == entity.ConditionName && x.ConditionId != id);
                if (exists)
                {
                    return new ResponseDto<ConditionDto>(false, "Condition name already exists.", null!);
                }

                string slug = StringUtilities.GenerateSlug(entity.ConditionName);
                bool slugExists = await _context.Conditions.AnyAsync(x => x.Slug == slug && x.ConditionId != id);
                if (slugExists)
                {
                    return new ResponseDto<ConditionDto>(false, "A condition with the same slug already exists.", null!);
                }

                conditionEntity.ConditionName = entity.ConditionName;
                conditionEntity.Slug = slug;
                conditionEntity.Description = entity.Description;
                conditionEntity.UpdatedAt = DateTime.UtcNow;

                _context.Conditions.Update(conditionEntity);
                await _context.SaveChangesAsync();

                var conditionDto = _mapper.Map<ConditionDto>(conditionEntity);

                return new ResponseDto<ConditionDto>(true, "Condition updated successfully.", conditionDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<ConditionDto>(false, $"An error occurred: {ex.Message}", null!);
            }
        }
    }
}
