using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.Interface
{
    public interface ICategoryService : IGenericService<CategoryDto, Guid>
    {
        Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsync(CategoryFilterDto Filter);
    }
}
