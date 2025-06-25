using System.Text.Json.Serialization;

namespace MyEverything.ThisMvc.Entities.DTOs
{
    public class LoginResponse_Dto
    {
        [JsonPropertyName("access-token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("access-token-expiration")]
        public DateTime AccessTokenExpiration { get; set; }

        [JsonPropertyName("refresh-token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("refresh-token-expiration")]
        public DateTime RefreshTokenExpiration { get; set; }

    }
}
