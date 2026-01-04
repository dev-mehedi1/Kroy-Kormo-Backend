namespace Sob_Need_Server.DTOs.Location
{
    public class LocationDto
    {
        public string? AddressLine { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public Guid? CityId { get; set; }
        public string? CityName { get; set; }
        public Guid? RegionId { get; set; }
        public string? RegionName { get; set; }
        public Guid? StateId { get; set; }
        public string? StateName { get; set; }
    }
}
