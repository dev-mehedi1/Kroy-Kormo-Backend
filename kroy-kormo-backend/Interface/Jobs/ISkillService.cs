using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;

namespace Sob_Need_Server.Interface.Jobs
{
    public interface ISkillService
    {
        Task<ResponseDto<IEnumerable<SkillDto>>> GetAllAsync(string? search = null);
        Task<ResponseDto<SkillDto>> GetByIdAsync(Guid id);
        Task<ResponseDto<SkillDto>> CreateAsync(SkillDto entity);
        Task<ResponseDto<SkillDto>> UpdateAsync(Guid id, SkillDto entity);
        Task<ResponseDto<bool>> DeleteAsync(Guid id);
    }
}
