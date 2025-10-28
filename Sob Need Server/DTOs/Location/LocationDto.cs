namespace Sob_Need_Server.DTOs.Location
{
    public class LocationDto
    {
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public int ProductId { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
    }
}
