using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.DTOs.Location
{
    public class CityDto
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public Guid StateId { get; set; }
    }
}
