using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.User;
using Sob_Need_Server.Interface.User;
using Sob_Need_Server.Utils;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Controllers.User
{
    [Route(Constants.BasePath + "/[controller]")]
    [ApiController]
    [Authorize]
    public class SavedSearchController(ISavedSearchService savedSearchService, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly ISavedSearchService _savedSearchService = savedSearchService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Save a search with filters
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ResponseDto<SavedSearchDto>>> SaveSearch([FromBody] SavedSearchDto savedSearchDto)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<SavedSearchDto>(false, "Unauthorized", null!));

            var response = await _savedSearchService.SaveSearchAsync(Guid.Parse(userIdStr), savedSearchDto);
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Get user's saved searches
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<SavedSearchDto>>>> GetSavedSearches()
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<IEnumerable<SavedSearchDto>>(false, "Unauthorized", null!));

            var response = await _savedSearchService.GetSavedSearchesAsync(Guid.Parse(userIdStr));
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        /// <summary>
        /// Delete a saved search
        /// </summary>
        [HttpDelete("{searchId}")]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteSavedSearch(Guid searchId)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new ResponseDto<bool>(false, "Unauthorized", false));

            var response = await _savedSearchService.DeleteSavedSearchAsync(searchId, Guid.Parse(userIdStr));
            
            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
