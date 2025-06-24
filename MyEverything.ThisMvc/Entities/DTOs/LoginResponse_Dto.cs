using System.Text.Json.Serialization;

namespace MyEverything.ThisMvc.Entities.DTOs
{
    public class LoginResponse_Dto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

    }
}
