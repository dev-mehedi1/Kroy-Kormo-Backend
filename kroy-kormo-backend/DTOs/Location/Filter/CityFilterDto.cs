using Sob_Need_Server.DTOs.Common;

namespace DTOs.Location.Filter;

public class CityFilterDto : FilterDto
{
    public Guid? StateId { get; set; }

}
