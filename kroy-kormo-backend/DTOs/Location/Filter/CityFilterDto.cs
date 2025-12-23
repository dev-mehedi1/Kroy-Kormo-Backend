using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.DTOs.Location.Filter;

public class CityFilterDto : FilterDto
{
    public int? StateId { get; set; }

}
