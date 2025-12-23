using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.DTOs.Location.Filter;

public class RegionFilterDto : FilterDto
{
    public Guid? CityId { get; set; }

}
