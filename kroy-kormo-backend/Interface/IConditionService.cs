using Sob_Need_Server.DTOs.Product;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface
{
    public interface IConditionService : IGenericService<ConditionDto, Guid>
    {
        Task<ResponseDto<IEnumerable<ConditionDto>>> GetAllAsync(ConditionFilterDto filter);
    }
}
