using System.Text.Json.Serialization;

namespace Services.Dtos;

public class Jwt
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";
}