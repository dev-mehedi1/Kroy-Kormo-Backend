using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Models.Users;

namespace Sob_Need_Server.Services.User
{
    public class SavedSearchService(AppDbContext context, IMapper mapper) : ISavedSearchService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<SavedSearchDto>> SaveSearchAsync(Guid userId, SavedSearchDto savedSearchDto)
        {
            try
            {
                var savedSearch = new SavedSearch
                {
                    UserId = userId,
                    SearchName = savedSearchDto.SearchName,
                    FilterJson = savedSearchDto.FilterJson,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.SavedSearches.Add(savedSearch);
                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<SavedSearchDto>(savedSearch);
                return new ResponseDto<SavedSearchDto>(true, "Search saved successfully", resultDto);
            }
            catch (Exception ex)
            {
                return new ResponseDto<SavedSearchDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<IEnumerable<SavedSearchDto>>> GetSavedSearchesAsync(Guid userId)
        {
            try
            {
                var savedSearches = await _context.SavedSearches
                    .Where(s => s.UserId == userId && s.IsActive)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync();

                var savedSearchDtos = _mapper.Map<IEnumerable<SavedSearchDto>>(savedSearches);
                return new ResponseDto<IEnumerable<SavedSearchDto>>(true, "Saved searches retrieved successfully", savedSearchDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<SavedSearchDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> DeleteSavedSearchAsync(Guid searchId, Guid userId)
        {
            try
            {
                var savedSearch = await _context.SavedSearches
                    .FirstOrDefaultAsync(s => s.SearchId == searchId && s.UserId == userId);

                if (savedSearch == null)
                    return new ResponseDto<bool>(false, "Saved search not found", false);

                _context.SavedSearches.Remove(savedSearch);
                await _context.SaveChangesAsync();

                return new ResponseDto<bool>(true, "Saved search deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }
    }
}
