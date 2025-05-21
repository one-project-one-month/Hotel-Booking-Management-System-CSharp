namespace HotelManagementSystem.Data.Data.FeatureModels
{
    public class Jwt
    {
        public string? Secret {get; set;}
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public required int ExpireTime { get; set; }
    }
}
