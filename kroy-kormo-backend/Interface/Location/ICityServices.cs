using DTOs.Location.Filter;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;

namespace Sob_Need_Server.Interface.Location;

public interface ICityServices : IGenericService<CityDto, Guid>
{
    Task<ResponseDto<IEnumerable<CityDto>>> GetAllAsync(CityFilterDto Filter);
}
