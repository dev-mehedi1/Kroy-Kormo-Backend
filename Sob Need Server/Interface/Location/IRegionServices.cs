using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Location;
using Sob_Need_Server.DTOs.Location.Filter;

namespace Sob_Need_Server.Interface.Location;

public interface IRegionServices : IGenericService<RegionDto, int>
{
    Task<ResponseDto<IEnumerable<RegionDto>>> GetAllAsync(RegionFilterDto Filter);
}
