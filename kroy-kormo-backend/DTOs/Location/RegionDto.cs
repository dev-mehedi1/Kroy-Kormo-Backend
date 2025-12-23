using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.DTOs.Location
{
    public class RegionDto
    {
        public Guid RegionId { get; set; }
        public string RegionName { get; set; }
        public Guid CityId { get; set; }
    }
}
