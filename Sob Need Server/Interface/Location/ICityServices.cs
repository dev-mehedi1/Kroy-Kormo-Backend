using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;

namespace Sob_Need_Server.Interface.Location;

public interface ICityServices : IGenericService<CityDto, int>
{
    Task<ResponseDto<IEnumerable<CityDto>>> GetAllAsync(CityFilterDto Filter);
}
