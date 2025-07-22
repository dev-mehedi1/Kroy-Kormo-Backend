using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.DTOs.Location
{
    public class RegionDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int CityId { get; set; }
    }
}
