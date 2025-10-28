using Sob_Need_Server.DTOs.Common;

namespace Sob_Need_Server.DTOs.Location.Filter
{
    public class StateFilterDto : FilterDto
    {
        public Guid? CountryId { get; set; }

    }
}
