using System.ComponentModel.DataAnnotations;

namespace Sob_Need_Server.DTOs.Location
{
    public class CityDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
    }
}
