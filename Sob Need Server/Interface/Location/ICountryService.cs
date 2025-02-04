using Sob_Need_Server.DTOs.Category;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;

namespace Sob_Need_Server.Interface.Location
{
    public interface ICountryService : IGenericService<CountryDto, int>
    {
        Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllAsync(FilterDto Filter);

    }
}
